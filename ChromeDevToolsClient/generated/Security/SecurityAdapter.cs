namespace Zu.ChromeDevTools.Security
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the Security domain to simplify the command interface.
    /// </summary>
    public class SecurityAdapter
    {
        private readonly ChromeSession m_session;
        
        public SecurityAdapter(ChromeSession session)
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
        /// Disables tracking security state changes.
        /// </summary>
        public async Task<DisableCommandResponse> Disable(DisableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DisableCommand, DisableCommandResponse>(command ?? new DisableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enables tracking security state changes.
        /// </summary>
        public async Task<EnableCommandResponse> Enable(EnableCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<EnableCommand, EnableCommandResponse>(command ?? new EnableCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enable/disable whether all certificate errors should be ignored.
        /// </summary>
        public async Task<SetIgnoreCertificateErrorsCommandResponse> SetIgnoreCertificateErrors(SetIgnoreCertificateErrorsCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetIgnoreCertificateErrorsCommand, SetIgnoreCertificateErrorsCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Handles a certificate error that fired a certificateError event.
        /// </summary>
        public async Task<HandleCertificateErrorCommandResponse> HandleCertificateError(HandleCertificateErrorCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<HandleCertificateErrorCommand, HandleCertificateErrorCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enable/disable overriding certificate errors. If enabled, all certificate error events need to
        /// be handled by the DevTools client and should be answered with `handleCertificateError` commands.
        /// </summary>
        public async Task<SetOverrideCertificateErrorsCommandResponse> SetOverrideCertificateErrors(SetOverrideCertificateErrorsCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetOverrideCertificateErrorsCommand, SetOverrideCertificateErrorsCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

        /// <summary>
        /// There is a certificate error. If overriding certificate errors is enabled, then it should be
        /// handled with the `handleCertificateError` command. Note: this event does not fire if the
        /// certificate error has been allowed internally. Only one client per target should override
        /// certificate errors at the same time.
        /// </summary>
        public void SubscribeToCertificateErrorEvent(Action<CertificateErrorEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// The security state of the page changed.
        /// </summary>
        public void SubscribeToVisibleSecurityStateChangedEvent(Action<VisibleSecurityStateChangedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// The security state of the page changed. No longer being sent.
        /// </summary>
        public void SubscribeToSecurityStateChangedEvent(Action<SecurityStateChangedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
    }
}