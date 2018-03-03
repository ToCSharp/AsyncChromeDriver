// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using System.Threading.Tasks;
using System.Collections.Concurrent;
using Zu.ChromeDevTools.DOM;

namespace Zu.Chrome.DriverCore
{
    public class DomTracker
    {
        private ChromeDevToolsConnection _devTools;
        private ConcurrentDictionary<long, string> _nodeToFrame = new ConcurrentDictionary<long, string>();
        public DomTracker(ChromeDevToolsConnection devTools)
        {
            _devTools = devTools;
        }

        public async Task<string> GetFrameIdForNode(int nodeId)
        {
            if (_nodeToFrame.TryGetValue(nodeId, out string res))
                return res;
            await _devTools.DOM.GetDocument(new GetDocumentCommand()).ConfigureAwait(false);
            if (_nodeToFrame.TryGetValue(nodeId, out string res2))
                return res2;
            //throw new KeyNotFoundException(frame);
            return null;
        }

        public async Task Enable()
        {
            _nodeToFrame.Clear();
            _devTools.DOM.SubscribeToSetChildNodesEvent(OnSetChildNodesEvent);
            _devTools.DOM.SubscribeToChildNodeInsertedEvent(OnChildNodeInsertedEvent);
            _devTools.DOM.SubscribeToDocumentUpdatedEvent(OnDocumentUpdatedEvent);
            await _devTools.DOM.Enable().ConfigureAwait(false);
            await _devTools.DOM.GetDocument(new GetDocumentCommand()).ConfigureAwait(false);
        }

        private void OnSetChildNodesEvent(SetChildNodesEvent ev)
        {
            ProcessNodeList(ev.Nodes);
        }

        private void ProcessNodeList(Node[] nodes)
        {
            if (nodes == null)
                return;
            foreach (var node in nodes)
            {
                ProcessNode(node);
            }
        }

        private void OnChildNodeInsertedEvent(ChildNodeInsertedEvent ev)
        {
            ProcessNode(ev.Node);
        }

        private void ProcessNode(Node node)
        {
            if (node == null)
                return;
            _nodeToFrame.AddOrUpdate(node.NodeId, node.FrameId, (key, oldValue) => node.FrameId);
            ProcessNodeList(node.Children);
        }

        private /*async*/ void OnDocumentUpdatedEvent(DocumentUpdatedEvent ev)
        {
            _nodeToFrame.Clear();
            _devTools?.DOM.GetDocument(new GetDocumentCommand());
        }
    }
}