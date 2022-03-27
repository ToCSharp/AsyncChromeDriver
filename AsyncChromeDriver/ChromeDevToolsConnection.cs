// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Zu.ChromeDevTools;

namespace Zu.Chrome
{
    public class ChromeDevToolsConnection
    {
        private bool _isDisconnected;
        private ChromeSession _session;

        public int Port {
            get;
            private set;
        }

        public virtual ChromeSession Session
        {
            get
            {
                if (_isDisconnected)
                    throw new Exception(nameof(ChromeDevToolsConnection) + " already disconnected");
                return _session;
            }
            set => _session = value;
        }

        #region Session properties
        //System.IO.File.WriteAllText("sess.txt", string.Join(Environment.NewLine, typeof(ChromeSession).GetProperties().Where(v => v.PropertyType.FullName.StartsWith("Zu.ChromeDevTools.")).Select(v => $"public {v.PropertyType.FullName.Substring("Zu.ChromeDevTools.".Length)} {v.Name} => Session.{v.Name};").ToList()))
        public ChromeDevTools.Accessibility.AccessibilityAdapter Accessibility => Session.Accessibility;
        public ChromeDevTools.Animation.AnimationAdapter Animation => Session.Animation;
        public ChromeDevTools.Audits.AuditsAdapter Audits => Session.Audits;
        public ChromeDevTools.BackgroundService.BackgroundServiceAdapter BackgroundService => Session.BackgroundService;
        public ChromeDevTools.Browser.BrowserAdapter Browser => Session.Browser;
        public ChromeDevTools.CacheStorage.CacheStorageAdapter CacheStorage => Session.CacheStorage;
        public ChromeDevTools.Cast.CastAdapter Cast => Session.Cast;
        public ChromeDevTools.Console.ConsoleAdapter Console => Session.Console;
        public ChromeDevTools.CSS.CSSAdapter CSS => Session.CSS;
        public ChromeDevTools.Database.DatabaseAdapter Database => Session.Database;
        public ChromeDevTools.Debugger.DebuggerAdapter Debugger => Session.Debugger;
        public ChromeDevTools.DeviceOrientation.DeviceOrientationAdapter DeviceOrientation => Session.DeviceOrientation;
        public ChromeDevTools.DOM.DOMAdapter DOM => Session.DOM;
        public ChromeDevTools.DOMDebugger.DOMDebuggerAdapter DOMDebugger => Session.DOMDebugger;
        public ChromeDevTools.DOMSnapshot.DOMSnapshotAdapter DOMSnapshot => Session.DOMSnapshot;
        public ChromeDevTools.DOMStorage.DOMStorageAdapter DOMStorage => Session.DOMStorage;
        public ChromeDevTools.Emulation.EmulationAdapter Emulation => Session.Emulation;
        public ChromeDevTools.EventBreakpoints.EventBreakpointsAdapter EventBreakpoints => Session.EventBreakpoints;
        public ChromeDevTools.Fetch.FetchAdapter Fetch => Session.Fetch;
        public ChromeDevTools.HeadlessExperimental.HeadlessExperimentalAdapter HeadlessExperimental => Session.HeadlessExperimental;
        public ChromeDevTools.HeapProfiler.HeapProfilerAdapter HeapProfiler => Session.HeapProfiler;
        public ChromeDevTools.IndexedDB.IndexedDBAdapter IndexedDB => Session.IndexedDB;
        public ChromeDevTools.Input.InputAdapter Input => Session.Input;
        public ChromeDevTools.Inspector.InspectorAdapter Inspector => Session.Inspector;
        public ChromeDevTools.IO.IOAdapter IO => Session.IO;
        public ChromeDevTools.LayerTree.LayerTreeAdapter LayerTree => Session.LayerTree;
        public ChromeDevTools.Log.LogAdapter Log => Session.Log;
        public ChromeDevTools.Memory.MemoryAdapter Memory => Session.Memory;
        public ChromeDevTools.Network.NetworkAdapter Network => Session.Network;
        public ChromeDevTools.Overlay.OverlayAdapter Overlay => Session.Overlay;
        public ChromeDevTools.Page.PageAdapter Page => Session.Page;
        public ChromeDevTools.Performance.PerformanceAdapter Performance => Session.Performance;
        public ChromeDevTools.PerformanceTimeline.PerformanceTimelineAdapter PerformanceTimeline => Session.PerformanceTimeline;
        public ChromeDevTools.Profiler.ProfilerAdapter Profiler => Session.Profiler;
        public ChromeDevTools.Runtime.RuntimeAdapter Runtime => Session.Runtime;
        public ChromeDevTools.Schema.SchemaAdapter Schema => Session.Schema;
        public ChromeDevTools.Security.SecurityAdapter Security => Session.Security;
        public ChromeDevTools.ServiceWorker.ServiceWorkerAdapter ServiceWorker => Session.ServiceWorker;
        public ChromeDevTools.Storage.StorageAdapter Storage => Session.Storage;
        public ChromeDevTools.SystemInfo.SystemInfoAdapter SystemInfo => Session.SystemInfo;
        public ChromeDevTools.Target.TargetAdapter Target => Session.Target;
        public ChromeDevTools.Tethering.TetheringAdapter Tethering => Session.Tethering;
        public ChromeDevTools.Tracing.TracingAdapter Tracing => Session.Tracing;
        public ChromeDevTools.WebAudio.WebAudioAdapter WebAudio => Session.WebAudio;
        public ChromeDevTools.WebAuthn.WebAuthnAdapter WebAuthn => Session.WebAuthn;
        #endregion

        public ChromeDevToolsConnection(int port = 5999)
        {
            Port = port;
        }

        public virtual async Task Connect()
        {
            var sessions = await GetSessions(Port).ConfigureAwait(false);
            var endpointUrl = sessions.FirstOrDefault(s => s.Type == "page")?.WebSocketDebuggerUrl;
            Session = new ChromeSession(endpointUrl);
        }

        // todo check
        public async Task<ChromeSessionInfo[]> GetSessions(int port)
        {
            //var remoteSessionUrls = new List<string>();
            var webClient = new HttpClient();
            var uriBuilder = new UriBuilder { Scheme = "http", Host = "127.0.0.1", Port = port, Path = "/json" };
            var remoteSessions = await webClient.GetStringAsync(uriBuilder.Uri).ConfigureAwait(false);
            try {
                //return JsonConvert.DeserializeObject<RemoteSession[]>(remoteSessions); 
                return JsonConvert.DeserializeObject<ChromeSessionInfo[]>(remoteSessions);
            } catch (Exception) {
                return null;
            }
        }

        public virtual void Disconnect()
        {
            Session?.Dispose();
            Session = null;
            _isDisconnected = true;
        }
        ////todo
    }
}