using System.Threading.Tasks;
using test_app.Generated.Elements;

namespace test_app.Generated.Reactive
{
    public class ReactiveText : IReactiveConsumer<string>
    {
        public ReactiveText(DependencyManager dependencyManager, JsManipulator jsManipulator, Element parentElement, IReactiveProvider<string> valueProvider)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            ParentElement = parentElement;
            ValueProvider = valueProvider;

            _dependencyManager.RegisterDependency(this, valueProvider);
        }

        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;
        
        public Element ParentElement { get; }
        public string Value => ValueProvider.Get();
        public IReactiveProvider<string> ValueProvider { get; private set; }
        
        public Task Changed(string oldValue, string newValue)
        {
            _jsManipulator.ReplaceContent(ParentElement.Id, oldValue, newValue);

            return Task.CompletedTask;
        }
    }
}