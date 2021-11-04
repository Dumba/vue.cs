using Microsoft.JSInterop;
using test_app.Base;
using test_app.Generated.Elements;

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
        public void ReplaceContent(string elementId, string oldText, string newText)
        {
            _js.InvokeVoidAsync("ReplaceContent", elementId, oldText, newText);
        }

        public void AddEventListener(string elementId, BaseComponent component, string eventName, string methodName, params object[] @params)
        {
            _js.InvokeVoidAsync("AddListener", elementId, eventName, component.ThisAsJsInterop, methodName, @params);
        }
    }
}