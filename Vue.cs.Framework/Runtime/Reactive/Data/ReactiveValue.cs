using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive.Data
{
    public class ReactiveValue<TValue> : IReactiveProvider<TValue>
    {
        public ReactiveValue(DependencyManager dependencyManager, TValue? value = default)
        {
            _dependencyManager = dependencyManager;
            Value = value;
        }

        private readonly DependencyManager _dependencyManager;

        public TValue? Value { get; private set; }

        public ValueTask Set(TValue value)
        {
            var oldValue = Value;
            Value = value;
            return _dependencyManager.ValueChanged(this, oldValue, Value);
        }
    }
}