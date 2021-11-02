using Microsoft.JSInterop;
using test_app.Components;

namespace test_app.Generated
{
    public class JsManipulator
    {
        public JsManipulator(IJSRuntime js)
        {
            _js = js;
        }

        public IJSRuntime _js;

        public void SetAttribute(string elementId, string attributeName, string attributeValue)
        {
            _js.InvokeVoidAsync("SetAttribute", elementId, attributeName, attributeValue);
        }
        public void RemoveAttribute(string elementId, string attributeName)
        {
            _js.InvokeVoidAsync("RemoveAttribute", elementId, attributeName);
        }
        public void InsertContent(string elementId, IElement element, int? elementIndex = null)
        {
            _js.InvokeVoidAsync("InsertContent", elementId, element, elementIndex);
        }
        public void RemoveContent(string elementId, int? elementIndex = null)
        {
            _js.InvokeVoidAsync("RemoveContent", elementId, elementIndex);
        }
        public void UpdateContent(string elementId, IElement element, int? elementIndex = null)
        {
            _js.InvokeVoidAsync("UpdateContent", elementId, element, elementIndex);
        }

        public void AddEventListener<TComponent>(string elementId, string eventName, TComponent component, string methodName) where TComponent : BaseComponent
        {
            _js.InvokeVoidAsync("AddListener", elementId, eventName, component.ThisAsJsInterop, methodName);
        }
    }
}