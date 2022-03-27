using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zu.ChromeDevTools.DOM;
using Zu.ChromeDevTools.Page;

namespace Zu.ChromeDevTools
{
    public static class Extensions
    {
        public static Task<DOM.EnableCommandResponse> Enable(this DOMAdapter DOM)
            => DOM.Enable(new DOM.EnableCommand());

        public static Task<CaptureScreenshotCommandResponse> CaptureScreenshot(this PageAdapter Page)
            => Page.CaptureScreenshot(new CaptureScreenshotCommand());
    }
}
