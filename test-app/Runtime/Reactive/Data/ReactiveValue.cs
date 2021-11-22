using System.Threading.Tasks;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.Data
{
    public class ReactiveValue<TValue> : IReactiveProvider<TValue>
        where TValue : class
    {
        public ReactiveValue(DependencyManager dependencyManager)
        {
            _dependencyManager = dependencyManager;
        }

        private readonly DependencyManager _dependencyManager;

        private TValue _value;
        public TValue Get(IReactiveConsumer<TValue> consumer)
        {
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