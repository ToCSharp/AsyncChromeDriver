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
    using WebSocket4Net;

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

            //m_sessionSocket = new WebSocket(m_endpointAddress);
            ////{
            ////    EnableAutoSendPing = false
            ////};
            //m_sessionSocket.MessageReceived += Ws_MessageReceived;
            //m_sessionSocket.Error += Ws_Error;
            //m_sessionSocket.Opened += Ws_Opened;
        }

        private ConcurrentDictionary<long, TaskCompletionSource<string>> _messages =
           new ConcurrentDictionary<long, TaskCompletionSource<string>>();
        public virtual async Task<string> SendCommand(string command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id = Interlocked.Increment(ref m_currentCommandId);

            var json = JToken.Parse(command);
            var commandId = json["id"]?.Value<int>();

            json["id"] = id;
            var commandWithNewId = json.ToString(Formatting.None);

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
            var contents = commandWithNewId;

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

            var resJson = JToken.Parse(res);
            var resId = resJson["id"]?.ToString();
            resJson["id"] = commandId;
            res = resJson.ToString(Formatting.None);
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


        SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private async Task OpenSessionConnection(CancellationToken cancellationToken)
        {
            try
            {
                await semaphore.WaitAsync();
                //var st = m_sessionSocket.State;
                if (m_sessionSocket == null || m_sessionSocket.State != WebSocketState.Open)
                {
                    if(m_sessionSocket != null)
                    {
                        m_sessionSocket.MessageReceived -= Ws_MessageReceived;
                        m_sessionSocket.Error -= Ws_Error;
                        m_sessionSocket.Opened -= Ws_Opened;
                        m_sessionSocket.Dispose();
                    }
                    m_sessionSocket = new WebSocket(m_endpointAddress);
                    //{
                    //    EnableAutoSendPing = false
                    //};
                    m_sessionSocket.MessageReceived += Ws_MessageReceived;
                    m_sessionSocket.Error += Ws_Error;
                    m_sessionSocket.Opened += Ws_Opened;

                    m_openEvent.Reset();
                    //m_sessionSocket.LocalEndPoint = null;

                    m_sessionSocket.Open();

                    await Task.Run(() => m_openEvent.Wait(cancellationToken));
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

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

        private void Ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            LogError("Error: {exception}", e.Exception);
            m_openEvent.Set();
            //throw e.Exception;
        }

        private void Ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                //var responseInfo = new ResponseInfo { Result = JToken.Parse(e.Data) };
                ProcessIncomingMessage(e.Message);
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
                        m_sessionSocket.Opened -= Ws_Opened;
                        m_sessionSocket.Error -= Ws_Error;
                        m_sessionSocket.MessageReceived -= Ws_MessageReceived;
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