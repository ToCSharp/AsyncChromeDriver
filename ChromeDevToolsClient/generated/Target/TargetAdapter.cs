namespace Zu.ChromeDevTools.Target
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the Target domain to simplify the command interface.
    /// </summary>
    public class TargetAdapter
    {
        private readonly ChromeSession m_session;
        
        public TargetAdapter(ChromeSession session)
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
        /// Activates (focuses) the target.
        /// </summary>
        public async Task<ActivateTargetCommandResponse> ActivateTarget(ActivateTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<ActivateTargetCommand, ActivateTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Attaches to the target with given id.
        /// </summary>
        public async Task<AttachToTargetCommandResponse> AttachToTarget(AttachToTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<AttachToTargetCommand, AttachToTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Attaches to the browser target, only uses flat sessionId mode.
        /// </summary>
        public async Task<AttachToBrowserTargetCommandResponse> AttachToBrowserTarget(AttachToBrowserTargetCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<AttachToBrowserTargetCommand, AttachToBrowserTargetCommandResponse>(command ?? new AttachToBrowserTargetCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Closes the target. If the target is a page that gets closed too.
        /// </summary>
        public async Task<CloseTargetCommandResponse> CloseTarget(CloseTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<CloseTargetCommand, CloseTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Inject object to the target's main frame that provides a communication
        /// channel with browser target.
        /// 
        /// Injected object will be available as `window[bindingName]`.
        /// 
        /// The object has the follwing API:
        /// - `binding.send(json)` - a method to send messages over the remote debugging protocol
        /// - `binding.onmessage = json => handleMessage(json)` - a callback that will be called for the protocol notifications and command responses.
        /// </summary>
        public async Task<ExposeDevToolsProtocolCommandResponse> ExposeDevToolsProtocol(ExposeDevToolsProtocolCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<ExposeDevToolsProtocolCommand, ExposeDevToolsProtocolCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Creates a new empty BrowserContext. Similar to an incognito profile but you can have more than
        /// one.
        /// </summary>
        public async Task<CreateBrowserContextCommandResponse> CreateBrowserContext(CreateBrowserContextCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<CreateBrowserContextCommand, CreateBrowserContextCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Returns all browser contexts created with `Target.createBrowserContext` method.
        /// </summary>
        public async Task<GetBrowserContextsCommandResponse> GetBrowserContexts(GetBrowserContextsCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetBrowserContextsCommand, GetBrowserContextsCommandResponse>(command ?? new GetBrowserContextsCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Creates a new page.
        /// </summary>
        public async Task<CreateTargetCommandResponse> CreateTarget(CreateTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<CreateTargetCommand, CreateTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Detaches session with given id.
        /// </summary>
        public async Task<DetachFromTargetCommandResponse> DetachFromTarget(DetachFromTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DetachFromTargetCommand, DetachFromTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Deletes a BrowserContext. All the belonging pages will be closed without calling their
        /// beforeunload hooks.
        /// </summary>
        public async Task<DisposeBrowserContextCommandResponse> DisposeBrowserContext(DisposeBrowserContextCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<DisposeBrowserContextCommand, DisposeBrowserContextCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Returns information about a target.
        /// </summary>
        public async Task<GetTargetInfoCommandResponse> GetTargetInfo(GetTargetInfoCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetTargetInfoCommand, GetTargetInfoCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Retrieves a list of available targets.
        /// </summary>
        public async Task<GetTargetsCommandResponse> GetTargets(GetTargetsCommand command = null, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<GetTargetsCommand, GetTargetsCommandResponse>(command ?? new GetTargetsCommand(), cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Sends protocol message over session with given id.
        /// Consider using flat mode instead; see commands attachToTarget, setAutoAttach,
        /// and crbug.com/991325.
        /// </summary>
        public async Task<SendMessageToTargetCommandResponse> SendMessageToTarget(SendMessageToTargetCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SendMessageToTargetCommand, SendMessageToTargetCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Controls whether to automatically attach to new targets which are considered to be related to
        /// this one. When turned on, attaches to all existing related targets as well. When turned off,
        /// automatically detaches from all currently attached targets.
        /// This also clears all targets added by `autoAttachRelated` from the list of targets to watch
        /// for creation of related targets.
        /// </summary>
        public async Task<SetAutoAttachCommandResponse> SetAutoAttach(SetAutoAttachCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetAutoAttachCommand, SetAutoAttachCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Adds the specified target to the list of targets that will be monitored for any related target
        /// creation (such as child frames, child workers and new versions of service worker) and reported
        /// through `attachedToTarget`. The specified target is also auto-attached.
        /// This cancels the effect of any previous `setAutoAttach` and is also cancelled by subsequent
        /// `setAutoAttach`. Only available at the Browser target.
        /// </summary>
        public async Task<AutoAttachRelatedCommandResponse> AutoAttachRelated(AutoAttachRelatedCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<AutoAttachRelatedCommand, AutoAttachRelatedCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Controls whether to discover available targets and notify via
        /// `targetCreated/targetInfoChanged/targetDestroyed` events.
        /// </summary>
        public async Task<SetDiscoverTargetsCommandResponse> SetDiscoverTargets(SetDiscoverTargetsCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetDiscoverTargetsCommand, SetDiscoverTargetsCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }
        /// <summary>
        /// Enables target discovery for the specified locations, when `setDiscoverTargets` was set to
        /// `true`.
        /// </summary>
        public async Task<SetRemoteLocationsCommandResponse> SetRemoteLocations(SetRemoteLocationsCommand command, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {
            return await m_session.SendCommand<SetRemoteLocationsCommand, SetRemoteLocationsCommandResponse>(command, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }

        /// <summary>
        /// Issued when attached to target because of auto-attach or `attachToTarget` command.
        /// </summary>
        public void SubscribeToAttachedToTargetEvent(Action<AttachedToTargetEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Issued when detached from target for any reason (including `detachFromTarget` command). Can be
        /// issued multiple times per target if multiple sessions have been attached to it.
        /// </summary>
        public void SubscribeToDetachedFromTargetEvent(Action<DetachedFromTargetEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Notifies about a new protocol message received from the session (as reported in
        /// `attachedToTarget` event).
        /// </summary>
        public void SubscribeToReceivedMessageFromTargetEvent(Action<ReceivedMessageFromTargetEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Issued when a possible inspection target is created.
        /// </summary>
        public void SubscribeToTargetCreatedEvent(Action<TargetCreatedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Issued when a target is destroyed.
        /// </summary>
        public void SubscribeToTargetDestroyedEvent(Action<TargetDestroyedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Issued when a target has crashed.
        /// </summary>
        public void SubscribeToTargetCrashedEvent(Action<TargetCrashedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
        /// <summary>
        /// Issued when some information about a target has changed. This only happens between
        /// `targetCreated` and `targetDestroyed`.
        /// </summary>
        public void SubscribeToTargetInfoChangedEvent(Action<TargetInfoChangedEvent> eventCallback)
        {
            m_session.Subscribe(eventCallback);
        }
    }
}