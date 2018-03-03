namespace Zu.ChromeDevTools
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using WebSocket4Net;

    /// <summary>
    /// Represents a websocket connection to a running chrome instance that can be used to send commands and recieve events.
    /// </summary>
    public partial class ChromeSession : IDisposable
    {
        private readonly string _endpointAddress;
        private readonly ILogger<ChromeSession> _logger;
        private readonly ConcurrentDictionary<string, ConcurrentBag<Action<object>>> _eventHandlers = new ConcurrentDictionary<string, ConcurrentBag<Action<object>>>();
        private readonly ConcurrentDictionary<Type, string> _eventTypeMap = new ConcurrentDictionary<Type, string>();

        private WebSocket _sessionSocket;
        private ManualResetEventSlim _openEvent = new ManualResetEventSlim(false);
        private long _currentCommandId = 0;

        public delegate void DevToolsEventHandler(object sender, string methodName, JToken eventData);
        public event DevToolsEventHandler DevToolsEvent;
        /// <summary>
        /// Gets or sets the number of milliseconds to wait for a command to complete. Default is 5 seconds.
        /// </summary>
        public int CommandTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the endpoint address of the session.
        /// </summary>
        public string EndpointAddress
        {
            get { return _endpointAddress; }
        }


        /// <summary>
        /// Creates a new Chrome session to the specified WS endpoint without logging.
        /// </summary>
        /// <param name="endpointAddress"></param>
        public ChromeSession(string endpointAddress)
            : this(null, endpointAddress)
        {
        }

        /// <summary>
        /// Creates a new ChromeSession to the specified WS endpoint with the specified logger implementation.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="endpointAddress"></param>
        public ChromeSession(ILogger<ChromeSession> logger, string endpointAddress)
            : this()
        {
            if (String.IsNullOrWhiteSpace(endpointAddress))
                throw new ArgumentNullException(nameof(endpointAddress));

            CommandTimeout = 5000;
            _logger = logger;
            _endpointAddress = endpointAddress;

            _sessionSocket = new WebSocket(_endpointAddress)
            {
                EnableAutoSendPing = false
            };
            _sessionSocket.MessageReceived += Ws_MessageReceived;
            _sessionSocket.Error += Ws_Error;
            _sessionSocket.Opened += Ws_Opened;
        }

        /// <summary>
        /// Sends the specified command and returns the associated command response.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="throwExceptionIfResponseNotReceived"></param>
        /// <returns></returns>
        public async Task<ICommandResponse<TCommand>> SendCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
            where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var result = await SendCommand(command.CommandName, JToken.FromObject(command), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);

            if (result == null)
                return null;

            if (!CommandResponseTypeMap.TryGetCommandResponseType<TCommand>(out Type commandResponseType))
                throw new InvalidOperationException($"Type {typeof(TCommand)} does not correspond to a known command response type.");

            return result.ToObject(commandResponseType) as ICommandResponse<TCommand>;
        }

        /// <summary>
        /// Sends the specified command and returns the associated command response.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam
        /// <typeparam name="TCommandResponse"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="throwExceptionIfResponseNotReceived"></param>
        /// <returns></returns>
        public async Task<TCommandResponse> SendCommand<TCommand, TCommandResponse>(TCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
            where TCommand : ICommand
            where TCommandResponse : ICommandResponse<TCommand>
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var result = await SendCommand(command.CommandName, JToken.FromObject(command), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);

            if (result == null)
                return default(TCommandResponse);

            return result.ToObject<TCommandResponse>();
        }

        private ConcurrentDictionary<long, TaskCompletionSource<ResponseInfo>> _messages =
           new ConcurrentDictionary<long, TaskCompletionSource<ResponseInfo>>();
        /// <summary>
        /// Returns a JToken based on a command created with the specified command name and params.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="params"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="throwExceptionIfResponseNotReceived"></param>
        /// <returns></returns>
        //[DebuggerStepThrough]
        public virtual async Task<JToken> SendCommand(string commandName, JToken @params, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            var id = Interlocked.Increment(ref _currentCommandId);
            var message = new
            {
                id = id,
                method = commandName,
                @params = @params
            };

            if (millisecondsTimeout.HasValue == false)
                millisecondsTimeout = CommandTimeout;

            await OpenSessionConnection(cancellationToken);

            LogTrace("Sending {id} {method}: {params}", message.id, message.method, @params?.ToString());

            var contents = JsonConvert.SerializeObject(message);

            if (_isDisposed) return null;
            ResponseInfo res = null;
            try
            {

                TaskCompletionSource<ResponseInfo> promise = _messages.GetOrAdd(id, i => new TaskCompletionSource<ResponseInfo>());
                _sessionSocket.Send(contents);
                cancellationToken.ThrowIfCancellationRequested();

                cancellationToken.Register(() => promise.TrySetCanceled(), false);

                res = await promise.Task.ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

            }
            finally
            {
                _messages.TryRemove(id, out _);
            }

            if (res.IsError)
            {
                var errorMessage = res.Result.Value<string>("message");
                var errorData = res.Result.Value<string>("data");

                var exceptionMessage = $"{commandName}: {errorMessage}";
                if (!String.IsNullOrWhiteSpace(errorData))
                    exceptionMessage = $"{exceptionMessage} - {errorData}";

                LogTrace("Recieved Error Response {id}: {message} {data}", message.id, message, errorData);
                throw new CommandResponseException(exceptionMessage)
                {
                    Code = res.Result.Value<long>("code")
                };
            }
            return res.Result;
        }

        /// <summary>
        /// Subscribes to the event associated with the given type.
        /// </summary>
        /// <typeparam name="TEvent">Event to subscribe to</typeparam>
        /// <param name="eventCallback"></param>
        public virtual void Subscribe<TEvent>(Action<TEvent> eventCallback)
            where TEvent : IEvent
        {
            if (eventCallback == null)
                throw new ArgumentNullException(nameof(eventCallback));

            var eventName = _eventTypeMap.GetOrAdd(
                typeof(TEvent),
                (type) =>
                {
                    if (!EventTypeMap.TryGetMethodNameForType<TEvent>(out string methodName))
                        throw new InvalidOperationException($"Type {typeof(TEvent)} does not correspond to a known event type.");

                    return methodName;
                });

            var callbackWrapper = new Action<object>(obj => eventCallback((TEvent)obj));
            _eventHandlers.AddOrUpdate(eventName,
                (m) => new ConcurrentBag<Action<object>>(new[] { callbackWrapper }),
                (m, currentBag) =>
                {
                    currentBag.Add(callbackWrapper);
                    return currentBag;
                });
        }

        private async Task OpenSessionConnection(CancellationToken cancellationToken)
        {
            if (_sessionSocket.State != WebSocketState.Open)
            {
                _sessionSocket.Open();

                await Task.Run(() => _openEvent.Wait(cancellationToken), cancellationToken);
            }
        }

        private void RaiseEvent(string methodName, JToken eventData)
        {
            DevToolsEvent?.Invoke(this, methodName, eventData);
            if (_eventHandlers.TryGetValue(methodName, out ConcurrentBag<Action<Object>> bag))
            {
                if (!EventTypeMap.TryGetTypeForMethodName(methodName, out Type eventType))
                    throw new InvalidOperationException($"Unknown {methodName} does not correspond to a known event type.");

                var typedEventData = eventData.ToObject(eventType);
                foreach (var callback in bag)
                {
                    callback(typedEventData);
                }
            }
        }

        private void ProcessIncomingMessage(ResponseInfo response)
        {
            if (!(response.Result is JObject messageObject)) return;
            if (messageObject.TryGetValue("id", out JToken idProperty))
            {
                var res = new ResponseInfo();
                if (messageObject.TryGetValue("error", out JToken errorProperty))
                {
                    res.IsError = true;
                    res.Result = errorProperty;
                }
                else
                {
                    res.Result = messageObject["result"];
                }

                long commandId = idProperty.Value<long>();
                if (_messages.TryGetValue(commandId, out var promise))
                {
                    promise.SetResult(res);
                }
                else
                {
                    Debug.Fail(string.Format(CultureInfo.CurrentCulture, "Invalid response identifier '{0}'", commandId));
                }
                LogTrace("Recieved Response {id}: {message}", commandId, res.Result.ToString());
                return;
            }

            if (messageObject.TryGetValue("method", out JToken methodProperty))
            {
                var method = methodProperty.Value<string>();
                var eventData = messageObject["params"];
                LogTrace("Recieved Event {method}: {params}", method, eventData.ToString());
                RaiseEvent(method, eventData);
                return;
            }

            //LogTrace("Recieved Other: {message}", message);
        }

        private void LogTrace(string message, params object[] args)
        {
            _logger?.LogTrace(message, args);
        }

        private void LogError(string message, params object[] args)
        {
            _logger?.LogError(message, args);
        }


        #region EventHandlers
        private void Ws_Opened(object sender, EventArgs e)
        {
            _openEvent.Set();
        }

        private void Ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            LogError("Error: {exception}", e.Exception);
            //throw e.Exception;
        }

        private void Ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                var responseInfo = new ResponseInfo { Result = JToken.Parse(e.Message) };
                ProcessIncomingMessage(responseInfo);
            }
            catch
            {
                // ignored
            }
        }
        #endregion

        #region IDisposable Support
        private bool _isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    //Clear all subscribed events.
                    _eventHandlers.Clear();
                    _eventTypeMap.Clear();

                    if (_sessionSocket != null)
                    {
                        _sessionSocket.Opened -= Ws_Opened;
                        _sessionSocket.Error -= Ws_Error;
                        _sessionSocket.MessageReceived -= Ws_MessageReceived;
                        _sessionSocket.Dispose();
                        _sessionSocket = null;
                    }

                    if (_openEvent != null)
                    {
                        _openEvent.Dispose();
                        _openEvent = null;
                    }

                }

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Disposes of the ChromeSession and frees all resources.
        ///</summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region Nested Classes
        private class ResponseInfo
        {
            public bool IsError = false;
            public JToken Result;
        }
        #endregion
    }
}
