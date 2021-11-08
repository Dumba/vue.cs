using System;
using System.Threading.Tasks;

namespace test_app.Generated.Reactive.Data
{
    public class ReactiveGetter<TValue, TSourceValue> : IReactiveConsumer<TSourceValue>, IReactiveProvider<TValue>
    {
        public ReactiveGetter(DependencyManager dependencyManager, IReactiveProvider<TSourceValue> source, Func<TSourceValue, TValue> getter)
        {
            _dependencyManager = dependencyManager;
            _getter = getter;
            _value = getter(source.Get());

            _dependencyManager.RegisterDependency<TSourceValue>(this, source);
        }

        private readonly DependencyManager _dependencyManager;
        private Func<TSourceValue, TValue> _getter;
        private TValue _value;

        public TValue Get()
        {
            return _value;
        }

        public ValueTask Changed(TSourceValue oldValue, TSourceValue newValue)
        {
            var oldValueComputed = _value;
            _value = _getter(newValue);
            return _dependencyManager.ValueChanged(this, oldValueComputed, _value);
        }
    }
}