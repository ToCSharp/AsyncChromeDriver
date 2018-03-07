namespace Zu.ChromeDevTools.Memory
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the Memory domain to simplify the command interface.
    /// </summary>
    public class MemoryAdapter
    {
        private readonly ChromeSession m_session;
        
        public MemoryAdapter(ChromeSession session)
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
        /// 
        /// </summary>
        public async Task<GetDOMCountersCommandResponse> GetDOMCounters(GetDOMCountersCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetDOMCountersCommand, GetDOMCountersCommandResponse>(command ?? new GetDOMCountersCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task<PrepareForLeakDetectionCommandResponse> PrepareForLeakDetection(PrepareForLeakDetectionCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<PrepareForLeakDetectionCommand, PrepareForLeakDetectionCommandResponse>(command ?? new PrepareForLeakDetectionCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enable/disable suppressing memory pressure notifications in all processes.
        /// </summary>
        public async Task<SetPressureNotificationsSuppressedCommandResponse> SetPressureNotificationsSuppressed(SetPressureNotificationsSuppressedCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetPressureNotificationsSuppressedCommand, SetPressureNotificationsSuppressedCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Simulate a memory pressure notification in all processes.
        /// </summary>
        public async Task<SimulatePressureNotificationCommandResponse> SimulatePressureNotification(SimulatePressureNotificationCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SimulatePressureNotificationCommand, SimulatePressureNotificationCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

    }
}