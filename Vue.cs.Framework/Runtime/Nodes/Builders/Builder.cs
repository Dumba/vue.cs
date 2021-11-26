using System;
using System.Collections.Generic;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Extensions;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive.Data;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;
using Vue.cs.Framework.Runtime.Reactive.PageItems;

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
            _attributes = new Dictionary<string, string?>();
            _reactiveAttributes = new Dictionary<string, IReactiveProvider<string?>>();
            _eventHandlers = new HashSet<EventHandlerData>();
            _conditions = new HashSet<IReactiveProvider<bool>>();

            _children = new List<IPageItem>();
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly BaseComponent _parentComponent;

        private string _tagName;
        private List<string> _classes;
        private Dictionary<string, string?> _attributes;
        private Dictionary<string, IReactiveProvider<string?>> _reactiveAttributes;
        private HashSet<EventHandlerData> _eventHandlers;
        private HashSet<IReactiveProvider<bool>> _conditions;
        private List<IPageItem> _children;

        #region Element data
        public Builder AddClass(string className)
        {
            _classes.Add(className);

            return this;
        }
        public Builder AddAttribute(string name, string value)
        {
            _attributes.Add(name, value);

            return this;
        }
        public Builder AddAttribute(string name, IReactiveProvider<string?> valueProvider)
        {
            _reactiveAttributes.Add(name, valueProvider);

            return this;
        }
        public Builder AddEventListener(string eventName, string methodName, params object[] @params)
        {
            var eventHandler = new EventHandlerData(eventName, _parentComponent.ThisAsJsInterop, methodName, @params);
            _eventHandlers.Add(eventHandler);

            return this;
        }

        public Builder AddCondition(IReactiveProvider<bool> condition)
        {
            _conditions.Add(condition);

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
            var id = Guid.NewGuid();
            var reactiveText = _serviceProvider.Get<ReactiveText.Builder>()
                .Build(id, textProvider);
            var child = new NodeText(textProvider, id);

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
            builder.GetNodeData(helperBuilder);

            component.Setup(builder, helperBuilder._children);

            _addChild(builder.Build());

            return this;
        }
        public Builder AddChild(string tagName, Action<Builder> setupChild, out IPageItem child)
        {
            var childBuilder = new Builder(_serviceProvider, _parentComponent, tagName);
            if (setupChild != null)
                setupChild(childBuilder);

            child = childBuilder.Build();
            _addChild(child);

            return this;
        }

        public Builder AddChildren<TItem>(IEnumerable<TItem> collection, string tagName, Action<Builder, TItem>? setupChild = null)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in collection)
            {
                templateBuilder.AddChild(tagName, setupChild is not null
                    ? builder => setupChild(builder, item)
                    : builder => { });
            }

            _addChild(templateBuilder.Build());

            return this;
        }

        public Builder AddChildren<TItem>(ReactiveCollection<TItem> collection, string tagName, Action<Builder, TItem>? setupChild = null)
        {
            var reactivePageMultiItem = _serviceProvider.Get<ReactivePageMultiItem<TItem>.Builder>()
                .Build(_parentComponent, tagName, setupChild);

            var collectionBuilder = reactivePageMultiItem.Init(collection);

            _addChild(collectionBuilder.Build());

            return this;
        }

        private void _addChild(IPageItem child)
        {
            _children.Add(child);
        }
        #endregion

        protected void GetNodeData(Builder builder)
        {
            // class
            _classes.AddRange(builder._classes);

            // attributes
            foreach (var attribute in builder._attributes)
            {
                _attributes[attribute.Key] = attribute.Value;
            }
            foreach (var rAttribute in builder._reactiveAttributes)
            {
                _reactiveAttributes[rAttribute.Key] = rAttribute.Value;
            }

            // event handlers
            foreach (var eventHandler in builder._eventHandlers)
            {
                _eventHandlers.Add(eventHandler);
            }

            // condition
            foreach (var condition in builder._conditions)
            {
                _conditions.Add(condition);
            }
        }

        public IPageItem Build()
        {
            // create pageItem
            var pageItem = CreatePageItem();

            // add children - template need children before attributes
            pageItem.InnerNodes = _children;

            // node data
            foreach (var className in _classes)
            {
                pageItem.AddClass(className);
            }
            foreach (var attribute in _attributes)
            {
                pageItem.AddAttribute(attribute);
            }
            foreach (var reactiveAttribute in _reactiveAttributes)
            {
                pageItem.AddReactiveAttribute(_serviceProvider, reactiveAttribute.Key, reactiveAttribute.Value);
            }
            foreach (var eventHandler in _eventHandlers)
            {
                pageItem.AddEventHandler(eventHandler);
            }
            foreach (var condition in _conditions)
            {
                pageItem.AddCondition(condition);
            }
            pageItem.BuildCondition(_serviceProvider);

            // finish
            return pageItem;
        }

        protected virtual IPageItemBuild CreatePageItem()
        {
            return new NodeElement(_tagName);
        }
    }
}