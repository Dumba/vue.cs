using System.Linq;
using System.Threading.Tasks;
using test_app.Runtime.Nodes;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive
{
    public class ReactivePageItem : IReactiveConsumer<bool>
    {

        public ReactivePageItem(JsManipulator jsManipulator)
        {
            _jsManipulator = jsManipulator;
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
                }
            }
            // hide
            else
            {
                foreach (var node in PageItem.Nodes)
                {
                    await _jsManipulator.ReplaceNode(new NodeComment($" placeholder of {node.Id} ", node.Id));
                }
            }
        }
    }
}