using System.Threading.Tasks;
using test_app.Generated.Nodes;

namespace test_app.Generated.Reactive.Visual
{
    public class ReactiveAttribute : IReactiveConsumer<string>
    {
        public ReactiveAttribute(DependencyManager dependencyManager, JsManipulator jsManipulator, ElementNode element, string name, IReactiveProvider<string> valueProvider)
        {
            _jsManipulator = jsManipulator;

            Element = element;
            Name = name;
            ValueProvider = valueProvider;

            dependencyManager.RegisterDependency(this, valueProvider);
        }

        private readonly JsManipulator _jsManipulator;
        
        public ElementNode Element { get; }
        public string Name { get; }
        public string Value => ValueProvider.Get();
        public IReactiveProvider<string> ValueProvider { get; private set; }
        
        public Task Changed(string oldValue, string newValue)
        {
            _jsManipulator.SetAttribute(Element.Id, Name, newValue);

            return Task.CompletedTask;
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

            public ReactiveAttribute Build(ElementNode element, string name, IReactiveProvider<string> valueProvider)
            {
                return new ReactiveAttribute(_dependencyManager, _jsManipulator, element, name, valueProvider);
            }
        }
    }
}