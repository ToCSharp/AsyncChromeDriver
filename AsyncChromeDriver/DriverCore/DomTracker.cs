// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using System.Linq;
using System.Threading.Tasks;
using BaristaLabs.ChromeDevTools.Runtime.Page;
using BaristaLabs.ChromeDevTools.Runtime.Runtime;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using BaristaLabs.ChromeDevTools.Runtime.DOM;
using System;
using System.Threading;

namespace Zu.Chrome.DriverCore
{
    public class DomTracker
    {
        private ChromeDevToolsConnection devTools;
        private ConcurrentDictionary<long, string> nodeToFrame = new ConcurrentDictionary<long, string>();

        public DomTracker(ChromeDevToolsConnection devTools)
        {
            this.devTools = devTools;
        }

        public async Task<string> GetFrameIdForNode(int nodeId)
        {
            if (nodeToFrame.TryGetValue(nodeId, out string res)) return res;
            await devTools?.Session.DOM.GetDocument(new GetDocumentCommand());
            if (nodeToFrame.TryGetValue(nodeId, out string res2)) return res2;
            //throw new KeyNotFoundException(frame);
            return null;
        }

        public async Task Enable()
        {
            nodeToFrame.Clear();
            devTools?.Session.DOM.SubscribeToSetChildNodesEvent(OnSetChildNodesEvent);
            devTools?.Session.DOM.SubscribeToChildNodeInsertedEvent(OnChildNodeInsertedEvent);
            devTools?.Session.DOM.SubscribeToDocumentUpdatedEvent(OnDocumentUpdatedEvent);
            await devTools?.Session.DOM.Enable();
            await devTools?.Session.DOM.GetDocument(new GetDocumentCommand());
        }

        private void OnSetChildNodesEvent(SetChildNodesEvent ev)
        {
            ProcessNodeList(ev.Nodes);
        }

        private void ProcessNodeList(Node[] nodes)
        {
            if (nodes == null) return;
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
            if (node == null) return;
            var nodeId = node.NodeId;
            nodeToFrame.AddOrUpdate(node.NodeId, node.FrameId, (key, oldValue) => node.FrameId);
            ProcessNodeList(node.Children);
        }

        private /*async*/ void OnDocumentUpdatedEvent(DocumentUpdatedEvent ev)
        {
            nodeToFrame.Clear();
            devTools?.Session.DOM.GetDocument(new GetDocumentCommand());
        }
    }
}
