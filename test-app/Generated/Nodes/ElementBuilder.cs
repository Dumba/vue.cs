using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated.Reactive;

namespace test_app.Generated.Nodes
{
    public class ElementBuilder : INodeBuilder, IReactiveConsumer<bool>
    {
        public ElementBuilder(DependencyManager dependencyManager, JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, string tagName)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            _parentComponent = parentComponent;
            _parentElementId = parentElementId;
            _childBuilders = new List<INodeBuilder>();
            _element = new Element(tagName);
        }

        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;

        private BaseComponent _parentComponent;
        private Guid _parentElementId;
        private List<INodeBuilder> _childBuilders;
        private Element _element;
        private IReactiveProvider<bool> _condition;

        public INode Node => _element;
        public bool IsOnPage => _condition?.Get() != false;
        public INodeBuilder NextNodeBuilder { get; set; }

        public ElementBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;
            _dependencyManager.RegisterDependency(this, condition);

            return this;
        }

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
        public ElementBuilder AddAttribute(string name, IReactiveProvider<string> valueProvider)
        {
            var reactiveAttribute = new ReactiveAttribute(_dependencyManager, _jsManipulator, _element, name, valueProvider);
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
            var childBuilder = new ElementBuilder(_dependencyManager, _jsManipulator, _parentComponent, _element.Id, tagName);
            if (setupChild != null)
                setupChild(childBuilder);
            _childBuilders.Add(childBuilder);

            return this;
        }

        public ElementBuilder AddChild(BaseComponent child, Action<ComponentToElementBuilder> setupChild = null)
        {
            var childBuilder = new ComponentToElementBuilder(_dependencyManager, _jsManipulator, child, _element.Id);
            if (setupChild != null)
                setupChild(childBuilder);
            _childBuilders.Add(childBuilder);

            return this;
        }

        public ElementBuilder AddText(string text)
        {
            var childBuilder = new TextNodeBuilder(_jsManipulator, _element.Id, text);
            _childBuilders.Add(childBuilder);

            return this;
        }

        public ElementBuilder AddText(IReactiveProvider<string> textProvider)
        {
            var childBuilder = new TextNodeBuilder(_jsManipulator, _element.Id, textProvider.Get());
            var reactiveText = new ReactiveText(_dependencyManager, _jsManipulator, childBuilder.Node, textProvider);
            _childBuilders.Add(childBuilder);

            return this;
        }

        public async Task InsertToDomAsync(Guid? insertBeforeNodeId = null)
        {
            // v-if
            if (_condition?.Get() == false)
                return;

            // tag with attributes
            _jsManipulator.InsertNode(_parentElementId, Node, insertBeforeNodeId);

            // events
            foreach (var item in _element.EventHandlers)
            {
                _jsManipulator.AddEventListener(_element.Id, _parentComponent, item.@event, item.componentMethod, item.@params);
            }

            // children
            INodeBuilder previousChildBuilder = new TextNodeBuilder(_jsManipulator, _element.Id, ""); // thrown away
            foreach (var childBuilder in _childBuilders)
            {
                await childBuilder.InsertToDomAsync();
                // add DOM children
                previousChildBuilder.NextNodeBuilder = childBuilder;
                previousChildBuilder = childBuilder;
            }
        }

        public Task Changed(bool oldValue, bool newValue)
        {
            // no change - ignore
            if (oldValue == newValue)
            {
                return Task.CompletedTask;
            }

            // insert
            if (newValue)
            {
                // find next visible node
                var nextVisibleNodeBuilder = NextNodeBuilder;
                while (nextVisibleNodeBuilder != null && !nextVisibleNodeBuilder.IsOnPage)
                {
                    nextVisibleNodeBuilder = nextVisibleNodeBuilder.NextNodeBuilder;
                }

                return InsertToDomAsync(nextVisibleNodeBuilder?.Node.Id);
            }
            // remove
            else
            {
                _jsManipulator.RemoveNode(_element.Id);
                return Task.CompletedTask;
            }
        }
    }
}