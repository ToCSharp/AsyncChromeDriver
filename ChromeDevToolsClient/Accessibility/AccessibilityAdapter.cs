namespace Zu.ChromeDevTools.Accessibility
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the Accessibility domain to simplify the command interface.
    /// </summary>
    public class AccessibilityAdapter
    {
        private readonly ChromeSession m_session;
        
        public AccessibilityAdapter(ChromeSession session)
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
        /// Disables the accessibility domain.
        /// </summary>
        public async Task<DisableCommandResponse> Disable(DisableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DisableCommand, DisableCommandResponse>(command ?? new DisableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enables the accessibility domain which causes `AXNodeId`s to remain consistent between method calls.
    /// This turns on accessibility for the page, which can impact performance until accessibility is disabled.
        /// </summary>
        public async Task<EnableCommandResponse> Enable(EnableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<EnableCommand, EnableCommandResponse>(command ?? new EnableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Fetches the accessibility node and partial accessibility tree for this DOM node, if it exists.
        /// </summary>
        public async Task<GetPartialAXTreeCommandResponse> GetPartialAXTree(GetPartialAXTreeCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetPartialAXTreeCommand, GetPartialAXTreeCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Fetches the entire accessibility tree
        /// </summary>
        public async Task<GetFullAXTreeCommandResponse> GetFullAXTree(GetFullAXTreeCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetFullAXTreeCommand, GetFullAXTreeCommandResponse>(command ?? new GetFullAXTreeCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

    }
}