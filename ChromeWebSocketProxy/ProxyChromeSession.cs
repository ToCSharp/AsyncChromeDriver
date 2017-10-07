//using BaristaLabs.ChromeDevTools.Runtime.Framework;

namespace Zu.ChromeWebSocketProxy
{
    //using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using WebSocketSharp;

    /// <summary>
    /// Represents a websocket connection to a running chrome instance that can be used to send commands and recieve events.
    ///</summary>
    public class ProxyChromeSession : IDisposable
    {
        private readonly string m_endpointAddress;
        private readonly ILogger<ProxyChromeSession> m_logger;
        private readonly ConcurrentDictionary<string, ConcurrentBag<Action<object>>> m_eventHandlers = new ConcurrentDictionary<string, ConcurrentBag<Action<object>>>();
        private readonly ConcurrentDictionary<Type, string> m_eventTypeMap = new ConcurrentDictionary<Type, string>();

        private WebSocket m_sessionSocket;
        private ManualResetEventSlim m_openEvent = new ManualResetEventSlim(false);
        private long m_currentCommandId = 0;

        public delegate void DevToolsEventHandler(object sender, string methodName, JToken eventData);
        public event DevToolsEventHandler DevToolsEvent;
        public event EventHandler<string> OnEvent;
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
            get { return m_endpointAddress; }
        }


        /// <summary>
        /// Creates a new Chrome session to the specified WS endpoint without logging.
        /// </summary>
        /// <param name="endpointAddress"></param>
        public ProxyChromeSession(string endpointAddress)
            : this(null, endpointAddress)
        {
        }

        /// <summary>
        /// Creates a new ChromeSession to the specified WS endpoint with the specified logger implementation.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="endpointAddress"></param>
        public ProxyChromeSession(ILogger<ProxyChromeSession> logger, string endpointAddress)
        {
            if (String.IsNullOrWhiteSpace(endpointAddress))
                throw new ArgumentNullException(nameof(endpointAddress));

            CommandTimeout = 5000;
            m_logger = logger;
            m_endpointAddress = endpointAddress;

            m_sessionSocket = new WebSocket(m_endpointAddress);
            //{
            //    EnableAutoSendPing = false
            //};
            m_sessionSocket.OnMessage += Ws_MessageReceived;
            m_sessionSocket.OnError += Ws_Error;
            m_sessionSocket.OnOpen += Ws_Opened;
        }

        ///// <summary>
        ///// Sends the specified command and returns the associated command response.
        ///// </summary>
        ///// <typeparam name="TCommand"></typeparam>
        ///// <param name="command"></param>
        ///// <param name="cancellationToken"></param>
        ///// <param name="millisecondsTimeout"></param>
        ///// <param name="throwExceptionIfResponseNotReceived"></param>
        ///// <returns></returns>
        //public async Task<ICommandResponse<TCommand>> SendCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        //    where TCommand : ICommand
        //{
        //    if (command == null)
        //        throw new ArgumentNullException(nameof(command));

        //    var result = await SendCommand(command.CommandName, JToken.FromObject(command), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);

        //    if (result == null)
        //        return null;

        //    if (!CommandResponseTypeMap.TryGetCommandResponseType<TCommand>(out Type commandResponseType))
        //        throw new InvalidOperationException($"Type {typeof(TCommand)} does not correspond to a known command response type.");

        //    return result.ToObject(commandResponseType) as ICommandResponse<TCommand>;
        //}

        ///// <summary>
        ///// Sends the specified command and returns the associated command response.
        ///// </summary>
        ///// <typeparam name="TCommand"></typeparam
        ///// <typeparam name="TCommandResponse"></typeparam>
        ///// <param name="command"></param>
        ///// <param name="cancellationToken"></param>
        ///// <param name="millisecondsTimeout"></param>
        ///// <param name="throwExceptionIfResponseNotReceived"></param>
        ///// <returns></returns>
        //public async Task<TCommandResponse> SendCommand<TCommand, TCommandResponse>(TCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        //    where TCommand : ICommand
        //    where TCommandResponse : ICommandResponse<TCommand>
        //{
        //    if (command == null)
        //        throw new ArgumentNullException(nameof(command));

        //    var result = await SendCommand(command.CommandName, JToken.FromObject(command), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);

        //    if (result == null)
        //        return default(TCommandResponse);

        //    return result.ToObject<TCommandResponse>();
        //}

        private ConcurrentDictionary<long, TaskCompletionSource<string>> _messages =
           new ConcurrentDictionary<long, TaskCompletionSource<string>>();
        ///// <summary>
        ///// Returns a JToken based on a command created with the specified command name and params.
        ///// </summary>
        ///// <param name="commandName"></param>
        ///// <param name="params"></param>
        ///// <param name="cancellationToken"></param>
        ///// <param name="millisecondsTimeout"></param>
        ///// <param name="throwExceptionIfResponseNotReceived"></param>
        ///// <returns></returns>
        ////[DebuggerStepThrough]
        public virtual async Task<string> SendCommand(string command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id = Interlocked.Increment(ref m_currentCommandId);
            //var message = new
            //{
            //    id = id,
            //    method = commandName,
            //    @params = @params
            //};

            //if (millisecondsTimeout.HasValue == false)
            //    millisecondsTimeout = CommandTimeout;

            await OpenSessionConnection(cancellationToken);

            //LogTrace("Sending {id} {method}: {params}", message.id, message.method, @params?.ToString());

            //var contents = JsonConvert.SerializeObject(message);
            var contents = command;

            if (m_isDisposed) return null;
            string res = null;
            try
            {

                TaskCompletionSource<string> promise = _messages.GetOrAdd(id, i => new TaskCompletionSource<string>());
                m_sessionSocket.Send(contents);
                cancellationToken.ThrowIfCancellationRequested();

                cancellationToken.Register(() => promise.TrySetCanceled(), false);

                res = await promise.Task.ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

            }
            finally
            {
                TaskCompletionSource<string> promise;
                _messages.TryRemove(id, out promise);
            }

            //if (res.IsError)
            //{
            //    var errorMessage = res.Result.Value<string>("message");
            //    var errorData = res.Result.Value<string>("data");

            //    var exceptionMessage = $"{commandName}: {errorMessage}";
            //    if (!String.IsNullOrWhiteSpace(errorData))
            //        exceptionMessage = $"{exceptionMessage} - {errorData}";

            //    LogTrace("Recieved Error Response {id}: {message} {data}", message.id, message, errorData);
            //    throw new CommandResponseException(exceptionMessage)
            //    {
            //        Code = res.Result.Value<long>("code")
            //    };
            //}
            return res;
        }

        ///// <summary>
        ///// Subscribes to the event associated with the given type.
        ///// </summary>
        ///// <typeparam name="TEvent">Event to subscribe to</typeparam>
        ///// <param name="eventCallback"></param>
        //public virtual void Subscribe<TEvent>(Action<TEvent> eventCallback)
        //    where TEvent : IEvent
        //{
        //    if (eventCallback == null)
        //        throw new ArgumentNullException(nameof(eventCallback));

        //    var eventName = m_eventTypeMap.GetOrAdd(
        //        typeof(TEvent),
        //        (type) =>
        //        {
        //            if (!EventTypeMap.TryGetMethodNameForType<TEvent>(out string methodName))
        //                throw new InvalidOperationException($"Type {typeof(TEvent)} does not correspond to a known event type.");

        //            return methodName;
        //        });

        //    var callbackWrapper = new Action<object>(obj => eventCallback((TEvent)obj));
        //    m_eventHandlers.AddOrUpdate(eventName,
        //        (m) => new ConcurrentBag<Action<object>>(new[] { callbackWrapper }),
        //        (m, currentBag) =>
        //        {
        //            currentBag.Add(callbackWrapper);
        //            return currentBag;
        //        });
        //}

        private async Task OpenSessionConnection(CancellationToken cancellationToken)
        {
            if (m_sessionSocket.ReadyState != WebSocketState.Open)
            {
                m_openEvent.Reset();
                m_sessionSocket.Connect();

                await Task.Run(() => m_openEvent.Wait(cancellationToken));
            }
        }

        //private void RaiseEvent(string methodName, JToken eventData)
        //{
        //    DevToolsEvent?.Invoke(this, methodName, eventData);
        //    if (m_eventHandlers.TryGetValue(methodName, out ConcurrentBag<Action<Object>> bag))
        //    {
        //        if (!EventTypeMap.TryGetTypeForMethodName(methodName, out Type eventType))
        //            throw new InvalidOperationException($"Unknown {methodName} does not correspond to a known event type.");

        //        var typedEventData = eventData.ToObject(eventType);
        //        foreach (var callback in bag)
        //        {
        //            callback(typedEventData);
        //        }
        //    }
        //}

        private void ProcessIncomingMessage(string mess)
        {
            var response = JToken.Parse(mess);
            var messageObject = response as JObject; 
            if (messageObject == null) return;
            if (messageObject.TryGetValue("id", out JToken idProperty))
            {
                //var res = new ResponseInfo();
                //if (messageObject.TryGetValue("error", out JToken errorProperty))
                //{
                //    res.IsError = true;
                //    res.Result = errorProperty;
                //}
                //else 
                //{
                //    res.Result = messageObject["result"];
                //}

                long commandId = idProperty.Value<long>();
                TaskCompletionSource<string> promise;
                if (_messages.TryGetValue(commandId, out promise))
                {
                    promise.SetResult(mess);
                }
                else
                {
                    Debug.Fail(string.Format(CultureInfo.CurrentCulture, "Invalid response identifier '{0}'", commandId));
                }
                //LogTrace("Recieved Response {id}: {message}", commandId, res.Result.ToString());
                return;
            }

            OnEvent?.Invoke(this, mess);
            //if (messageObject.TryGetValue("method", out JToken methodProperty))
            //{
            //    var method = methodProperty.Value<string>();
            //    var eventData = messageObject["params"];
            //    LogTrace("Recieved Event {method}: {params}", method, eventData.ToString());
                
            //    RaiseEvent(method, eventData);
            //    return;
            //}

            //LogTrace("Recieved Other: {message}", message);
        }

        private void LogTrace(string message, params object[] args)
        {
            if (m_logger == null)
                return;

            m_logger.LogTrace(message, args);
        }

        private void LogError(string message, params object[] args)
        {
            if (m_logger == null)
                return;

            m_logger.LogError(message, args);
        }


        #region EventHandlers
        private void Ws_Opened(object sender, EventArgs e)
        {
            m_openEvent.Set();
        }

        private void Ws_Error(object sender, ErrorEventArgs e)
        {
            LogError("Error: {exception}", e.Exception);
            //throw e.Exception;
        }

        private void Ws_MessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                //var responseInfo = new ResponseInfo { Result = JToken.Parse(e.Data) };
                ProcessIncomingMessage(e.Data);
            }
            catch { }
        }
        #endregion

        #region IDisposable Support
        private bool m_isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                if (disposing)
                {
                    //Clear all subscribed events.
                    m_eventHandlers.Clear();
                    m_eventTypeMap.Clear();

                    if (m_sessionSocket != null)
                    {
                        m_sessionSocket.OnOpen -= Ws_Opened;
                        m_sessionSocket.OnError -= Ws_Error;
                        m_sessionSocket.OnMessage -= Ws_MessageReceived;
                        m_sessionSocket.Close();
                        m_sessionSocket = null;
                    }

                    if (m_openEvent != null)
                    {
                        m_openEvent.Dispose();
                        m_openEvent = null;
                    }

                }

                m_isDisposed = true;
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