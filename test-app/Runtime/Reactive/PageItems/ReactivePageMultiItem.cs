using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using test_app.Base;
using test_app.Runtime.Nodes;
using test_app.Runtime.Nodes.Builders;
using test_app.Runtime.Reactive.Data;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.PageItems
{
    public class ReactivePageMultiItem<TItem> : IReactiveCollectionConsumer<TItem>
    {
        public ReactivePageMultiItem(IServiceProvider serviceProvider, BaseComponent parentComponent, string tagName, Action<ElementBuilder, TItem> setupChild)
        {
            _serviceProvider = serviceProvider;
            _parentComponent = parentComponent;
            _tagName = tagName;
            _setupChild = setupChild;
            _mapping = new Dictionary<TItem, IPageItem>();
        }

        private readonly IServiceProvider _serviceProvider;
        private BaseComponent _parentComponent;
        private string _tagName;
        private Action<ElementBuilder, TItem> _setupChild;
        private Dictionary<TItem, IPageItem> _mapping;

        public Guid TemplateId { get; set; }

        public TemplateBuilder Init(IEnumerable<TItem> initCollection)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in initCollection)
            {
                templateBuilder.AddChild(_tagName, elementBuilder => _setupChild(elementBuilder, item), out var pageItem);

                _mapping.Add(item, pageItem);
            }

            return templateBuilder;
        }
        public TemplateBuilder Init(ReactiveCollection<TItem> initCollection)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in initCollection.Get(this))
            {
                templateBuilder.AddChild(_tagName, elementBuilder => _setupChild(elementBuilder, item), out var pageItem);

                _mapping.Add(item, pageItem);
            }

            return templateBuilder;
        }

        public async ValueTask Added(TItem value)
        {
            var jsManipulator = _serviceProvider.GetService<JsManipulator>();
            var builder = new ElementBuilder(_serviceProvider, _parentComponent, _tagName);
            _setupChild(builder, value);
            var pageItem = builder.Build();

            _mapping.Add(value, pageItem);
            foreach (var node in pageItem.Nodes)
            {
                await jsManipulator.InsertNode(node, TemplateId);
            }
        }

        public async ValueTask Removed(TItem value)
        {
            var jsManipulator = _serviceProvider.GetService<JsManipulator>();
            var pageItem = _mapping[value];
            _mapping.Remove(value);

            foreach (var node in pageItem.Nodes)
            {
                await jsManipulator.RemoveNode(node.Id);
            }
        }

        public class Builder
        {
            public Builder(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            private IServiceProvider _serviceProvider;

            public ReactivePageMultiItem<TItem> Build(BaseComponent parentComponent, string tagName, Action<ElementBuilder, TItem> setupChild)
            {
                return new ReactivePageMultiItem<TItem>(_serviceProvider, parentComponent, tagName, setupChild);
            }
        }
    }
}