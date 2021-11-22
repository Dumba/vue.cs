
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using test_app.Base;
using test_app.Generated.Nodes;

namespace test_app.Runtime.Reactive
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
        public ValueTask InsertNode(Guid parentElementId, INode node, Guid? insertBeforeNodeId = null)
        {
            // Guid.Empty = Master component
            return _js.InvokeVoidAsync("InsertNode", parentElementId != Guid.Empty ? parentElementId : Program.ParentSelector, node, insertBeforeNodeId);
        }
        public ValueTask RemoveNode(Guid nodeId)
        {
            return _js.InvokeVoidAsync("RemoveNode", nodeId);
        }
        public ValueTask UpdateText(Guid nodeId, string newText)
        {
            return _js.InvokeVoidAsync("UpdateText", nodeId, newText);
        }

        // events
        public ValueTask AddEventListener(Guid elementId, BaseComponent component, string eventName, string methodName, params object[] @params)
        {
            return _js.InvokeVoidAsync("AddListener", elementId, eventName, component.ThisAsJsInterop, methodName, @params);
        }
    }
}