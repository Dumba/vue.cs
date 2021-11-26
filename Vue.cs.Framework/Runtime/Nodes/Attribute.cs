using System;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class Attribute : IReactiveConsumer<string?>
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

        private DependencyManager? _dependencyManager;
        private JsManipulator? _jsManipulator;
        private Guid? _ownerElementId;
        private string? _value;
        private IReactiveProvider<string?>? _reactiveValue;

        public string Name { get; }
        public string? Value => _reactiveValue is not null ? _reactiveValue.Value : _value;
        
        public object Build(DependencyManager dependencyManager, JsManipulator jsManipulator, Guid ownerElementId)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;
            _ownerElementId = ownerElementId;

            if (_reactiveValue is not null)
            {
                dependencyManager.RegisterDependency(this, _reactiveValue);
            }

            return new
            {
                Name,
                Value,
            };
        }
        public void Demolish()
        {
            if (_reactiveValue is not null)
                _dependencyManager?.UnregisterDependency(this, _reactiveValue);

            _dependencyManager = null;
            _jsManipulator = null;
            _ownerElementId = null;
        }

        public async ValueTask Changed(string? oldValue, string? newValue)
        {
            if (_jsManipulator is null || _ownerElementId is null)
                return;

            await _jsManipulator.SetAttribute(_ownerElementId.Value, Name, newValue);
        }
    }
}