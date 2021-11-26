
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive
{
    public class JsManipulator
    {
        public JsManipulator(IJSRuntime js)
        {
            _js = js;
        }

        public IJSRuntime _js;

        // attributes
        public ValueTask SetAttribute(Guid elementId, string attributeName, string attributeValue)
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
            return _js.InvokeVoidAsync("InsertNode", parentElementId, node, null);
        }
        public ValueTask InsertNode(string parentElementSelector, IPageNode node)
        {
            // Guid.Empty = Master component
            return _js.InvokeVoidAsync("InsertNode", parentElementSelector, node, null);
        }
        public ValueTask InsertNodeBefore(IPageNode node, Guid insertBeforeNodeId)
        {
            return _js.InvokeVoidAsync("InsertNodeBefore", node, insertBeforeNodeId);
        }
        public ValueTask ReplaceNode(IPageNode pageItem)
        {
            return _js.InvokeVoidAsync("ReplaceNode", pageItem.Id, pageItem);
        }
        public ValueTask RemoveNode(Guid nodeId)
        {
            return _js.InvokeVoidAsync("RemoveNode", nodeId);
        }
        public ValueTask UpdateText(Guid nodeId, string newText)
        {
            return _js.InvokeVoidAsync("UpdateText", nodeId, newText);
        }
    }
}