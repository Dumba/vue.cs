using System;
using System.Linq;
using test_app.Base;
using test_app.Generated.Reactive;
using test_app.Generated.Reactive.Visual;

namespace test_app.Generated.Nodes
{
    public class ElementBuilder : INodeBuilder
    {
        public ElementBuilder(DependencyManager dependencyManager, JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, string tagName)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            _element = new ElementNode(tagName);
            _parentComponent = parentComponent;

            Node = new NodePositioned(_element, parentComponent, parentElementId);
        }

        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;

        private ElementNode _element;
        private BaseComponent _parentComponent;
        private IReactiveProvider<bool> _condition;

        public INodePositioned Node { get; private set; }

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

        public ElementBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;

            return this;
        }

        public ElementBuilder AddText(string text)
        {
            var child = new NodePositioned(new TextNode(text), _parentComponent, _element.Id);

            _addChild(child);

            return this;
        }
        public ElementBuilder AddText(IReactiveProvider<string> textProvider)
        {
            var child = new NodePositioned(new TextNode(textProvider.Get()), _parentComponent, _element.Id);
            var reactiveText = new ReactiveText(_dependencyManager, _jsManipulator, child.Node, textProvider);

            _addChild(child);

            return this;
        }

        public ElementBuilder AddChild(string tagName, Action<ElementBuilder> setupChild = null)
        {
            var childBuilder = new ElementBuilder(_dependencyManager, _jsManipulator, _parentComponent, _element.Id, tagName);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }
        public ElementBuilder AddChild(BaseComponent child, Action<ComponentBuilder> setupChild = null)
        {
            var childBuilder = new ComponentBuilder(_dependencyManager, _jsManipulator, child, _element.Id);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }

        private void _addChild(INodePositioned child)
        {
            var prevChild = Node.Children.LastOrDefault();
            if (prevChild != null)
            {
                prevChild.NextNode = child;
                child.PrevNode = prevChild;
            }
            Node.Children.Add(child);
        }

        // public ElementBuilder AddChildren<TItem>(IEnumerable<TItem> collection, string tagName, Action<ElementBuilder, TItem> setupChild = null)
        // {
        //     foreach (var item in collection)
        //     {
        //         var childBuilder = new ElementBuilder(_dependencyManager, _jsManipulator, _parentComponent, _element.Id, tagName);
        //         if (setupChild != null)
        //             setupChild(childBuilder, item);

        //         _childBuilders.Add(childBuilder);
        //     }

        //     return this;
        // }

        // public ElementBuilder AddChildren<TItem>(ReactiveList<TItem> collection, string tagName, Action<ElementBuilder, TItem> setupChild = null)
        // {
        //     foreach (var item in collection)
        //     {
        //         var childBuilder = new ElementBuilder(_dependencyManager, _jsManipulator, _parentComponent, _element.Id, tagName);
        //         if (setupChild != null)
        //             setupChild(childBuilder, item);

        //         _childBuilders.Add(childBuilder);
        //     }

        //     return this;
        // }

        public INodePositioned Build()
        {
            if (_condition != null)
            {
                Node = new ReactiveNode(_dependencyManager, Node, _condition);
            }

            return Node;
        }
    }
}