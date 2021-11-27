
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
        public async ValueTask InsertNode(string parentElementSelector, IPageItem pageItem)
        {
            // Master component
            foreach (var node in pageItem.Build(_dependencyManager, this))
            {
                await _js.InvokeVoidAsync("InsertNode", parentElementSelector, node, null);
            }
        }
        public async ValueTask InsertNode(Guid parentElementId, IPageItem pageItem)
        {
            foreach (var node in pageItem.Build(_dependencyManager, this))
            {
                await _js.InvokeVoidAsync("InsertNode", parentElementId, node, null);
            }
        }
        public async ValueTask InsertNodeBefore(IPageItem pageItem, Guid insertBeforeNodeId)
        {
            foreach (var node in pageItem.Build(_dependencyManager, this))
            {
                await _js.InvokeVoidAsync("InsertNodeBefore", node, insertBeforeNodeId);
            }
        }
        public ValueTask ReplaceNode(INodeBuilt node)
        {
            return _js.InvokeVoidAsync("ReplaceNode", node.Id, node);
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