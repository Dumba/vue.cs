using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated.Reactive;

namespace test_app.Generated.Elements
{
    public class ElementBuilder : IElementBuilder
    {
        public ElementBuilder(string tagName, string id = null)
        {
            _childBuilders = new List<IElementBuilder>();

            _element = new Element(tagName, id);
        }

        private List<IElementBuilder> _childBuilders;
        private Element _element;

        public IElement Element => _element;

        public ElementBuilder AddClass(string className)
        {
            _element.Classes.Add(className);

            return this;
        }

        public ElementBuilder AddAttribute(string name, string value)
        {
            _element.Attributes.Add(name, value);

            return this;
        }
        public ElementBuilder AddAttribute(string name, IReactiveProvider<string> valueProvider, DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            var reactiveAttribute = new ReactiveAttribute(dependencyManager, jsManipulator, _element, name, valueProvider);
            _element.Attributes.Add(name, reactiveAttribute.Value);

            return this;
        }

        public ElementBuilder AddEventListener(string eventName, string methodName, params object[] @params)
        {
            _element.EventHandlers.Add((eventName, methodName, @params));

            return this;
        }

        public ElementBuilder AddChild(string tagName, string id = null, Action<ElementBuilder> setupChild = null)
        {
            var childBuilder = new ElementBuilder(tagName);
            if (setupChild != null)
                setupChild(childBuilder);
            _childBuilders.Add(childBuilder);

            return this;
        }

        public ElementBuilder AddChild(BaseComponent child)
        {
            _childBuilders.Add(new ComponentToElementBuilder(child));

            return this;
        }

        public ElementBuilder AddText(string text)
        {
            _childBuilders.Add(new TextElementBuilder(text));

            return this;
        }

        public ElementBuilder AddText(IReactiveProvider<string> textProvider, DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            var reactiveText = new ReactiveText(dependencyManager, jsManipulator, _element, textProvider);
            _childBuilders.Add(new TextElementBuilder(reactiveText.Value));

            return this;
        }

        public async Task InsertToDomAsync(JsManipulator jsManipulator, string parentId, BaseComponent parentComponent)
        {
            // tag with attributes
            jsManipulator.InsertContent(parentId, Element);

            // events
            foreach (var item in _element.EventHandlers)
            {
                jsManipulator.AddEventListener(_element.Id, parentComponent, item.@event, item.componentMethod, item.@params);
            }

            // children
            foreach (var childBuilder in _childBuilders)
            {
                await childBuilder.InsertToDomAsync(jsManipulator, _element.Id, parentComponent);
            }
        }
    }
}