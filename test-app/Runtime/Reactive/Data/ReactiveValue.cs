using System.Threading.Tasks;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.Data
{
    public class ReactiveValue<TValue> : IReactiveProvider<TValue>
    {
        public ReactiveValue(DependencyManager dependencyManager, TValue value = default)
        {
            _dependencyManager = dependencyManager;
            _value = value;
        }

        private readonly DependencyManager _dependencyManager;

        private TValue _value;
        public TValue Get(IReactiveConsumer<TValue> consumer)
        {
            if (consumer is not null)
                _dependencyManager.RegisterDependency(consumer, this);
                
            return _value;
        }
        public ValueTask Set(TValue value)
        {
            var oldValue = _value;
            _value = value;
            return _dependencyManager.ValueChanged(this, oldValue, value);
        }
    }
}