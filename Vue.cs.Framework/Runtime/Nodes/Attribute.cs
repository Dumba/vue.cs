using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class Attribute
    {
        public Attribute(string name, string? value)
        {
            Name = name;
            _value = value;
        }
        public Attribute(string name, IReactiveProvider<string?> reactiveValue)
        {
            Name = name;
            _reactiveValue = reactiveValue;
        }

        private string? _value;
        private IReactiveProvider<string?>? _reactiveValue;

        public string Name { get; }
        public string? Value => _reactiveValue is not null ? _reactiveValue.Value : _value;
    }
}