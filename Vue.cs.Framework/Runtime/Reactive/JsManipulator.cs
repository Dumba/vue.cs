
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive
{
    public class JsManipulator
    {
        public JsManipulator(IJSRuntime js, DependencyManager dependencyManager)
        {
            _js = js;
            _dependencyManager = dependencyManager;
        }

        private readonly IJSRuntime _js;
        private readonly DependencyManager _dependencyManager;

        // attributes
        public ValueTask SetAttribute(Guid elementId, string attributeName, string? attributeValue)
        {
            return _js.InvokeVoidAsync("SetAttribute", elementId, attributeName, attributeValue);
        }
        public ValueTask RemoveAttribute(Guid elementId, string attributeName)
        {
            return _js.InvokeVoidAsync("RemoveAttribute", elementId, attributeName);
        }

        // nodes
        public ValueTask InsertNode(Guid parentElementId, IPageNode node)
        {
            // Guid.Empty = Master component
            return _js.InvokeVoidAsync("InsertNode", parentElementId, node.Build(_dependencyManager, this), null);
        }
        public ValueTask InsertNode(string parentElementSelector, IPageNode node)
        {
            // Guid.Empty = Master component
            return _js.InvokeVoidAsync("InsertNode", parentElementSelector, node.Build(_dependencyManager, this), null);
        }
        public ValueTask InsertNodeBefore(IPageNode node, Guid insertBeforeNodeId)
        {
            return _js.InvokeVoidAsync("InsertNodeBefore", node.Build(_dependencyManager, this), insertBeforeNodeId);
        }
        public ValueTask ReplaceNode(IPageNode pageItem)
        {
            return _js.InvokeVoidAsync("ReplaceNode", pageItem.Id, pageItem.Build(_dependencyManager, this));
        }
        public ValueTask RemoveNode(Guid nodeId)
        {
            return _js.InvokeVoidAsync("RemoveNode", nodeId);
        }
        public ValueTask UpdateText(Guid nodeId, string? newText)
        {
            return _js.InvokeVoidAsync("UpdateText", nodeId, newText);
        }
    }
}