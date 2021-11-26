using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Reactive.Data;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;
using Vue.cs.Framework.Runtime.Nodes;
using Vue.cs.Framework.Runtime.Nodes.Builders;
using PageItemBuilder = Vue.cs.Framework.Runtime.Nodes.Builders.Builder;

namespace Vue.cs.Framework.Runtime.Reactive.PageItems
{
    public class ReactivePageMultiItem<TItem> : IReactiveCollectionConsumer<TItem>
    {
        public ReactivePageMultiItem(IServiceProvider serviceProvider, BaseComponent parentComponent, string tagName, Action<PageItemBuilder, TItem> setupChild)
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
        private Action<PageItemBuilder, TItem> _setupChild;
        private Dictionary<TItem, IPageItem> _mapping;

        public Guid TemplateEndId { get; set; }

        public PageItemBuilder Init(IEnumerable<TItem> initCollection)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in initCollection)
            {
                templateBuilder.AddChild(_tagName, elementBuilder => _setupChild(elementBuilder, item), out var pageItem);

                _mapping.Add(item, pageItem);
            }

            return templateBuilder;
        }
        public PageItemBuilder Init(ReactiveCollection<TItem> initCollection)
        {
            var templateBuilder = new TemplateBuilder(_serviceProvider, _parentComponent);

            foreach (var item in initCollection.Get(this))
            {
                templateBuilder.AddChild(_tagName, elementBuilder => _setupChild(elementBuilder, item), out var pageItem);

                _mapping.Add(item, pageItem);
            }

            TemplateEndId = templateBuilder.EndNode.Id;
            return templateBuilder;
        }

        public async ValueTask Added(TItem value)
        {
            var jsManipulator = _serviceProvider.GetService<JsManipulator>();
            var builder = new PageItemBuilder(_serviceProvider, _parentComponent, _tagName);
            _setupChild(builder, value);
            var pageItem = builder.Build();

            _mapping.Add(value, pageItem);
            foreach (var node in pageItem.Nodes)
            {
                await jsManipulator.InsertNodeBefore(node, TemplateEndId);

                if (node.IsVisible && node is INodeParent element)
                {
                    foreach (var child in element.Children)
                    {
                        await child.Render(jsManipulator, node.Id);
                    }
                }
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

            public ReactivePageMultiItem<TItem> Build(BaseComponent parentComponent, string tagName, Action<PageItemBuilder, TItem> setupChild)
            {
                return new ReactivePageMultiItem<TItem>(_serviceProvider, parentComponent, tagName, setupChild);
            }
        }
    }
}