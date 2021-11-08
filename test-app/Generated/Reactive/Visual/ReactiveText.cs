using System.Threading.Tasks;
using test_app.Generated.Nodes;

namespace test_app.Generated.Reactive.Visual
{
    public class ReactiveText : IReactiveConsumer<string>
    {
        public ReactiveText(DependencyManager dependencyManager, JsManipulator jsManipulator, INode node, IReactiveProvider<string> valueProvider)
        {
            _jsManipulator = jsManipulator;

            Node = node;
            ValueProvider = valueProvider;

            dependencyManager.RegisterDependency(this, valueProvider);
        }

        private readonly JsManipulator _jsManipulator;
        
        public INode Node { get; }
        public string Value => ValueProvider.Get();
        public IReactiveProvider<string> ValueProvider { get; private set; }
        
        public ValueTask Changed(string oldValue, string newValue)
        {
            return _jsManipulator.UpdateText(Node.Id, newValue);
        }

        public class Builder
        {
            public Builder(DependencyManager dependencyManager, JsManipulator jsManipulator)
            {
                _dependencyManager = dependencyManager;
                _jsManipulator = jsManipulator;
            }

            private readonly DependencyManager _dependencyManager;
            private readonly JsManipulator _jsManipulator;

            public ReactiveText Build(INode node, IReactiveProvider<string> valueProvider)
            {
                return new ReactiveText(_dependencyManager, _jsManipulator, node, valueProvider);
            }
        }
    }
}