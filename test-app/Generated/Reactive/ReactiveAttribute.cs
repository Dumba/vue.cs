using System.Threading.Tasks;
using test_app.Generated.Elements;

namespace test_app.Generated.Reactive
{
    public class ReactiveAttribute : IReactiveConsumer<string>
    {
        public ReactiveAttribute(DependencyManager dependencyManager, JsManipulator jsManipulator, Element element, string name, IReactiveProvider<string> valueProvider)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            Element = element;
            Name = name;
            ValueProvider = valueProvider;

            _dependencyManager.RegisterDependency(this, valueProvider);
        }

        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;
        
        public Element Element { get; }
        public string Name { get; }
        public string Value => ValueProvider.Get();
        public IReactiveProvider<string> ValueProvider { get; private set; }
        
        public Task Changed(string oldValue, string newValue)
        {
            _jsManipulator.SetAttribute(Element.Id, Name, newValue);

            return Task.CompletedTask;
        }
    }
}