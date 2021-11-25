using System.Threading.Tasks;
using test_app.Runtime.Nodes;
using test_app.Runtime.Nodes.Interfaces;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.PageItems
{
    public class ReactivePageItem : IReactiveConsumer<bool>
    {

        public ReactivePageItem(JsManipulator jsManipulator, IPageItem pageItem)
        {
            _jsManipulator = jsManipulator;

            PageItem = pageItem;
        }

        private readonly JsManipulator _jsManipulator;

        public IPageItem PageItem { get; }

        public async ValueTask Changed(bool oldValue, bool newValue)
        {
            // show
            if (newValue)
            {
                foreach (var node in PageItem.Nodes)
                {
                    await _jsManipulator.ReplaceNode(node);

                    if (node is INodeParent element)
                    {
                        foreach (var child in element.Children)
                        {
                            await child.Render(_jsManipulator, node.Id);
                        }
                    }
                }
            }
            // hide
            else
            {
                foreach (var node in PageItem.Nodes)
                {
                    await _jsManipulator.ReplaceNode(new NodeComment(id: node.Id));
                }
            }
        }

        public class Builder
        {
            public Builder(JsManipulator jsManipulator)
            {
                _jsManipulator = jsManipulator;
            }

            private readonly JsManipulator _jsManipulator;

            public ReactivePageItem Build(IPageItem pageItem, IReactiveProvider<bool> valueProvider, out bool initValue)
            {
                var reactivePageItem = new ReactivePageItem(_jsManipulator, pageItem);
                initValue = valueProvider.Get(reactivePageItem);
                return reactivePageItem;
            }
        }
    }
}