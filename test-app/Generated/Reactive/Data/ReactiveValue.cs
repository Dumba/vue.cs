using System.Threading.Tasks;

namespace test_app.Generated.Reactive.Data
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

        public TValue Get()
        {
            return _value;
        }

        public Task Set(TValue newValue)
        {
            var oldValue = _value;
            _value = newValue;
            return _dependencyManager.ValueChanged(this, oldValue, newValue);
        }
    }
}