
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using test_app.Base;
using test_app.Runtime.Nodes;

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
        public async ValueTask InsertNode(Guid parentElementId, IPageNode node)
        {
            // Guid.Empty = Master component
            await _js.InvokeVoidAsync("InsertNode", parentElementId != Guid.Empty ? parentElementId : Program.ParentSelector, node, null);

            if (node is NodeElement element)
            {
                foreach (var eventHandler in element.EventHandlers)
                {
                    await AddEventListener(element.Id, eventHandler.Component, eventHandler.Event, eventHandler.ComponentMethodName, eventHandler.Params);
                }
            }
        }
        public async ValueTask InsertNodeBefore(IPageNode node, Guid insertBeforeNodeId)
        {
            await _js.InvokeVoidAsync("InsertNodeBefore", node, insertBeforeNodeId);
            
            if (node is NodeElement element)
            {
                foreach (var eventHandler in element.EventHandlers)
                {
                    await AddEventListener(element.Id, eventHandler.Component, eventHandler.Event, eventHandler.ComponentMethodName, eventHandler.Params);
                }
            }
        }
        public ValueTask RemoveNode(Guid nodeId)
        {
            return _js.InvokeVoidAsync("RemoveNode", nodeId);
        }
        public ValueTask ReplaceNode(IPageNode pageItem)
        {
            return _js.InvokeVoidAsync("ReplaceNode", pageItem.Id, pageItem);
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