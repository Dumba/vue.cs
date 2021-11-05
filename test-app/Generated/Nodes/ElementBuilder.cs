using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated.Reactive;

namespace test_app.Generated.Nodes
{
    public class ElementBuilder : INodeBuilder
    {
        public ElementBuilder(string tagName)
        {
            _childBuilders = new List<INodeBuilder>();

            _element = new Element(tagName);
        }

        private List<INodeBuilder> _childBuilders;
        private Element _element;

        public INode Node => _element;

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

        public ElementBuilder AddChild(string tagName, Action<ElementBuilder> setupChild = null)
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
            _childBuilders.Add(new TextNodeBuilder(text));

            return this;
        }

        public ElementBuilder AddText(IReactiveProvider<string> textProvider, DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            var newNodeBuilder = new TextNodeBuilder(textProvider.Get());
            var reactiveText = new ReactiveText(dependencyManager, jsManipulator, newNodeBuilder.Node, textProvider);
            _childBuilders.Add(newNodeBuilder);

            return this;
        }

        public async Task InsertToDomAsync(JsManipulator jsManipulator, Guid parentId, BaseComponent parentComponent)
        {
            // tag with attributes
            jsManipulator.InsertNode(parentId, Node);

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