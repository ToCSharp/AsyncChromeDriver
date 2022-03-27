namespace Zu.ChromeDevTools.DOM
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the DOM domain to simplify the command interface.
    /// </summary>
    public partial class DOMAdapter
    {

        /// <summary>
        /// Enables DOM agent for the given page.
        /// </summary>
        public Task<EnableCommandResponse> Enable()
            => Enable(new EnableCommand());
    }
}