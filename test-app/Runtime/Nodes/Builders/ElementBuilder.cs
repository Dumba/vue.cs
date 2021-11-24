using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using test_app.Base;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Data;
using test_app.Runtime.Reactive.Interfaces;
using test_app.Runtime.Reactive.PageItems;

namespace test_app.Runtime.Nodes.Builders
{
    public class ElementBuilder : IBuilder
    {
        public ElementBuilder(IServiceProvider serviceProvider, BaseComponent parentComponent, string tagName)
        {
            _serviceProvider = serviceProvider;
            _parentComponent = parentComponent;
            _element = new NodeElement(tagName);
        }

        private readonly IServiceProvider _serviceProvider;
        private BaseComponent _parentComponent;
        private NodeElement _element;

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
            var reactiveAttribute = _serviceProvider.GetService<ReactiveAttribute.Builder>()
                .Build(_element.Id, name, valueProvider, out var text);
            _element.Attributes.Add(name, text);

            return this;
        }
        public ElementBuilder AddEventListener(string eventName, string methodName, params object[] @params)
        {
            _element.EventHandlers.Add(new EventHandlerData { Event = eventName, ComponentInterop = _parentComponent.ThisAsJsInterop, ComponentMethodName = methodName, Params = @params });

            return this;
        }

        public ElementBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _element.Condition = condition;
            return this;
        }

        public ElementBuilder AddText(string text)
        {
            var child = new NodeText(text);

            _addChild(child);

            return this;
        }
        public ElementBuilder AddText(IReactiveProvider<string> textProvider)
        {
            var id = Guid.NewGuid();
            var reactiveText = _serviceProvider.GetService<ReactiveText.Builder>()
                .Build(id, textProvider, out var text);
            var child = new NodeText(text, id);
            _addChild(child);

            return this;
        }

        public ElementBuilder AddChild(string tagName, Action<ElementBuilder> setupChild = null)
        {
            var childBuilder = new ElementBuilder(_serviceProvider, _parentComponent, tagName);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }
        public ElementBuilder AddChild<TComponent>(Action<ComponentBuilder<TComponent>> setupChild = null)
            where TComponent : BaseComponent
        {
            var childBuilder = new ComponentBuilder<TComponent>(_serviceProvider);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }

        private void _addChild(IPageItem child)
        {
            _element.Children.Add(child);
        }

        public ElementBuilder AddChildren<TItem>(IEnumerable<TItem> collection, string tagName, Action<ElementBuilder, TItem> setupChild = null)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in collection)
            {
                templateBuilder.AddChild(tagName, builder => setupChild(builder, item));
            }

            _addChild(templateBuilder.Build());

            return this;
        }

        public ElementBuilder AddChildren<TItem>(ReactiveCollection<TItem> collection, string tagName, Action<ElementBuilder, TItem> setupChild = null)
        {
            var reactivePageMultiItem = _serviceProvider.GetService<ReactivePageMultiItem<TItem>.Builder>()
                .Build(_parentComponent, tagName, setupChild);
                
            var collectionBuilder = reactivePageMultiItem.Init(collection);

            _addChild(collectionBuilder.Build());

            return this;
        }

        public IPageItem Build()
        {
            if (_element.Condition != null)
            {
                var reactiveNode = _serviceProvider.GetService<ReactivePageItem.Builder>()
                    .Build(_element, _element.Condition, out bool visible);
            }

            return _element;
        }
    }
}