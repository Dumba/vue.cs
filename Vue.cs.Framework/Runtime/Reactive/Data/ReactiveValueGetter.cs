using System;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive.Data
{
    public class ReactiveValueGetter<TIn, TOut> : IReactiveConsumer<TIn>, IReactiveProvider<TOut>
    {
        public ReactiveValueGetter(DependencyManager dependencyManager, Func<TIn?, TOut?> getter, IReactiveProvider<TIn> valueProvider)
        {
            _dependencyManager = dependencyManager;
            _getter = getter;
            var valueIn = valueProvider.Get(this);
            _value = _getter(valueIn);
        }

        private readonly DependencyManager _dependencyManager;

        private Func<TIn?, TOut?> _getter;
        private TOut? _value;
        public TOut? Get(IReactiveConsumer<TOut>? consumer)
        {
            if (consumer is not null)
                _dependencyManager.RegisterDependency(consumer, this);

            return _value;
        }
        public ValueTask Changed(TIn? oldValue, TIn? newValue)
        {
            var oldOutValue = _value;
            _value = _getter(newValue);

            return _dependencyManager.ValueChanged(this, oldOutValue, _value);
        }

        public class Builder
        {
            public Builder(DependencyManager dependencyManager)
            {
                _dependencyManager = dependencyManager;
            }

            private readonly DependencyManager _dependencyManager;

            public ReactiveValueGetter<TIn, TOut> Build(Func<TIn?, TOut?> getter, IReactiveProvider<TIn> valueProvider)
            {
                return new ReactiveValueGetter<TIn, TOut>(_dependencyManager, getter, valueProvider);
            }
        }
    }
}