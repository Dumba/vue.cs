using System;
using System.Collections.Generic;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Extensions;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive.Data;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes.Builders
{
    public class Builder
    {
        public Builder(IServiceProvider serviceProvider, BaseComponent parentComponent, string tagName)
        {
            _serviceProvider = serviceProvider;
            _parentComponent = parentComponent;

            _tagName = tagName;
            _classes = new List<string>();
            _attributes = new HashSet<Attribute>();
            _eventHandlers = new HashSet<EventHandlerData>();

            _children = new List<IPageItem>();
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly BaseComponent _parentComponent;

        private string _tagName;
        private List<string> _classes;
        private HashSet<Attribute> _attributes;
        private HashSet<EventHandlerData> _eventHandlers;
        private IReactiveProvider<bool>? _condition;
        private List<IPageItem> _children;

        #region Element data
        public Builder AddClass(string className)
        {
            _classes.Add(className);

            return this;
        }
        public Builder AddAttribute(string name, string value)
        {
            _attributes.Add(new Attribute(name, value));

            return this;
        }
        public Builder AddAttribute(string name, IReactiveProvider<string?> valueProvider)
        {
            _attributes.Add(new Attribute(name, valueProvider));

            return this;
        }
        public Builder AddEventListener(string eventName, string methodName, params object[] @params)
        {
            var eventHandler = new EventHandlerData(eventName, _parentComponent.ThisAsJsInterop, methodName, @params);
            _eventHandlers.Add(eventHandler);

            return this;
        }

        public Builder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition ?? _condition;

            return this;
        }
        #endregion

        #region Children
        public Builder AddText(string text)
        {
            var child = new NodeText(text);

            _addChild(child);

            return this;
        }
        public Builder AddText(IReactiveProvider<string?> textProvider)
        {
            var child = new NodeText(textProvider);

            _addChild(child);

            return this;
        }

        public Builder AddChild(string tagName, Action<Builder>? setupChild = null)
        {
            var childBuilder = new Builder(_serviceProvider, _parentComponent, tagName);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }
        public Builder AddChild<TComponent>(Action<Builder>? setupChild = null)
            where TComponent : BaseComponent
        {
            var component = _serviceProvider.Get<TComponent>();
            var builder = new TemplateBuilder(_serviceProvider, component);

            var helperBuilder = new TemplateBuilder(_serviceProvider, component);
            if (setupChild is not null)
                setupChild(helperBuilder);
            builder._getNodeData(helperBuilder);

            component.Setup(builder, helperBuilder._children);

            _addChild(builder.Build());

            return this;
        }

        public Builder AddChildren<TItem>(IEnumerable<TItem> collection, string tagName, Action<Builder, TItem>? setupChild = null)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in collection)
            {
                templateBuilder.AddChild(
                    tagName,
                    setupChild is not null
                        ? builder => setupChild(builder, item)
                        : null);
            }

            _addChild(templateBuilder.Build());

            return this;
        }

        public Builder AddChildren<TItem>(ReactiveCollection<TItem> collection, string tagName, Action<Builder, TItem>? setupChild = null)
        {
            var nodeCollection = new NodeCollection<TItem>(collection, i => _createChild(i, tagName, setupChild));

            _addChild(nodeCollection);

            return this;
        }

        private void _addChild(IPageItem child)
        {
            _children.Add(child);
        }

        private IPageItem _createChild<TItem>(TItem item, string tagName, Action<Builder, TItem>? setupChild)
        {
            var builder = new Builder(_serviceProvider, _parentComponent, tagName);
            if (setupChild is not null)
                setupChild(builder, item);
            var pageItem = builder.Build();
            return pageItem;
        }
        #endregion

        private void _getNodeData(Builder builder)
        {
            // class
            _classes.AddRange(builder._classes);

            // attributes
            foreach (var attribute in builder._attributes)
            {
                _attributes.Add(attribute);
            }

            // event handlers
            foreach (var eventHandler in builder._eventHandlers)
            {
                _eventHandlers.Add(eventHandler);
            }

            // condition
            _condition = builder._condition ?? _condition;
        }

        public IPageItem Build()
        {
            // create pageItem
            var pageItem = CreatePageItem();

            // add children - template need children before attributes
            pageItem.InnerNodes = _children;

            // node data
            foreach (var node in pageItem.Nodes)
            {
                if (node is NodeElement element)
                {
                    foreach (var className in _classes)
                    {
                        element.Classes.Add(className);
                    }
                    foreach (var attribute in _attributes)
                    {
                        element.Attributes.Add(attribute);
                    }
                    foreach (var eventHandler in _eventHandlers)
                    {
                        element.EventHandlers.Add(eventHandler);
                    }
                    element.Condition = _condition ?? element.Condition;
                }
            }

            // finish
            return pageItem;
        }

        protected virtual IPageItemCollection CreatePageItem()
        {
            return new NodeElement(_tagName);
        }
    }
}