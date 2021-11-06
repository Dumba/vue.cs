
using System;
using Microsoft.JSInterop;
using test_app.Base;
using test_app.Generated.Nodes;

namespace test_app.Generated
{
    public class JsManipulator
    {
        public JsManipulator(IJSRuntime js)
        {
            _js = js;
        }

        public IJSRuntime _js;

        // attributes
        public void SetAttribute(Guid elementId, string attributeName, string attributeValue)
        {
            _js.InvokeVoidAsync("SetAttribute", elementId, attributeName, attributeValue);
        }
        public void RemoveAttribute(Guid elementId, string attributeName)
        {
            _js.InvokeVoidAsync("RemoveAttribute", elementId, attributeName);
        }

        // nodes
        public void InsertNode(Guid parentElementId, INode node, Guid? insertBeforeNodeId = null)
        {
            // Guid.Empty = Master component
            _js.InvokeVoidAsync("InsertNode", parentElementId != Guid.Empty ? parentElementId : Program.ParentSelector, node.Serialize(), insertBeforeNodeId);
        }
        public void RemoveNode(Guid nodeId)
        {
            _js.InvokeVoidAsync("RemoveNode", nodeId);
        }
        public void UpdateText(Guid nodeId, string newText)
        {
            _js.InvokeVoidAsync("UpdateText", nodeId, newText);
        }

        // events
        public void AddEventListener(Guid elementId, BaseComponent component, string eventName, string methodName, params object[] @params)
        {
            _js.InvokeVoidAsync("AddListener", elementId, eventName, component.ThisAsJsInterop, methodName, @params);
        }
    }
}