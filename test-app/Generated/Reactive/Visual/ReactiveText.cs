using System.Threading.Tasks;
using test_app.Generated.Nodes;

namespace test_app.Generated.Reactive
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
        
        public Task Changed(string oldValue, string newValue)
        {
            _jsManipulator.UpdateText(Node.Id, newValue);

            return Task.CompletedTask;
        }
    }
}