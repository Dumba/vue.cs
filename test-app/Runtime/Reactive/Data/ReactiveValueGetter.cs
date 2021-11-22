using System;
using System.Threading.Tasks;
using test_app.Runtime.Reactive;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.ReactiveData
{
    public class ReactiveValueGetter<TIn, TOut> : IReactiveConsumer<TIn>, IReactiveProvider<TOut>
        where TIn : class
        where TOut : class
    {
        public ReactiveValueGetter(DependencyManager dependencyManager, Func<TIn, TOut> getter)
        {
            _dependencyManager = dependencyManager;
            _getter = getter;
        }

        private readonly DependencyManager _dependencyManager;

        private Func<TIn, TOut> _getter;
        private TOut _value;
        public TOut Value => _value;

        public ValueTask Changed(TIn oldValue, TIn newValue)
        {
            var oldOutValue = _value;
            _value = _getter(newValue);

            return _dependencyManager.ValueChanged(this, oldOutValue, _value);
        }
    }
}