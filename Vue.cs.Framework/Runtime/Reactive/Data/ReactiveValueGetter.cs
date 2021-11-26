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
            Value = _getter(valueProvider.Value);
        }

        private readonly DependencyManager _dependencyManager;
        private Func<TIn?, TOut?> _getter;

        public TOut? Value { get; private set; }

        public ValueTask Changed(TIn? oldValue, TIn? newValue)
        {
            var oldOutValue = Value;
            Value = _getter(newValue);

            return _dependencyManager.ValueChanged(this, oldOutValue, Value);
        }
    }
}