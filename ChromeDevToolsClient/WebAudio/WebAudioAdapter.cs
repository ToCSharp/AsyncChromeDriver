namespace Zu.ChromeDevTools.WebAudio
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the WebAudio domain to simplify the command interface.
    /// </summary>
    public class WebAudioAdapter
    {
        private readonly ChromeSession m_session;
        
        public WebAudioAdapter(ChromeSession session)
        {
            m_session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <summary>
        /// Gets the ChromeSession associated with the adapter.
        /// </summary>
        public ChromeSession Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// Enables the WebAudio domain and starts sending context lifetime events.
        /// </summary>
        public async Task<EnableCommandResponse> Enable(EnableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<EnableCommand, EnableCommandResponse>(command ?? new EnableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Disables the WebAudio domain.
        /// </summary>
        public async Task<DisableCommandResponse> Disable(DisableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DisableCommand, DisableCommandResponse>(command ?? new DisableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Fetch the realtime data from the registered contexts.
        /// </summary>
        public async Task<GetRealtimeDataCommandResponse> GetRealtimeData(GetRealtimeDataCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetRealtimeDataCommand, GetRealtimeDataCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

        /// <summary>
        /// Notifies that a new BaseAudioContext has been created.
        /// </summary>
        public void SubscribeToContextCreatedEvent(Action<ContextCreatedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Notifies that existing BaseAudioContext has been destroyed.
        /// </summary>
        public void SubscribeToContextDestroyedEvent(Action<ContextDestroyedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Notifies that existing BaseAudioContext has changed some properties (id stays the same)..
        /// </summary>
        public void SubscribeToContextChangedEvent(Action<ContextChangedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
    }
}