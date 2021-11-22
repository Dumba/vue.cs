using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Runtime.Nodes;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive
{
    public class ReactivePageMultiItem<TItem> : IReactiveCollectionConsumer<TItem>
    {
        public ReactivePageMultiItem(JsManipulator jsManipulator, Func<TItem, IPageItem> builder)
        {
            _jsManipulator = jsManipulator;
            _builder = builder;
            _mapping = new Dictionary<TItem, IPageItem>();
        }

        private readonly JsManipulator _jsManipulator;
        private Func<TItem, IPageItem> _builder;
        private Dictionary<TItem, IPageItem> _mapping;

        public Guid TemplateId { get; set; }

        public async ValueTask Added(TItem value)
        {
            var pageItem = _builder(value);
            _mapping.Add(value, pageItem);

            foreach (var node in pageItem.Nodes)
            {
                await _jsManipulator.InsertNode(node, TemplateId);
            }
        }

        public async ValueTask Removed(TItem value)
        {
            var pageItem = _mapping[value];
            _mapping.Remove(value);

            foreach (var node in pageItem.Nodes)
            {
                await _jsManipulator.RemoveNode(node.Id);
            }
        }
    }
}