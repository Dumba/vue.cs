using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Runtime.Reactive;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.ReactiveData
{
    public class ReactiveCollectionGetter<TItemIn, TItemOut> : IReactiveCollectionConsumer<TItemIn>, IReactiveCollectionProvider<TItemOut>
    {
        public ReactiveCollectionGetter(DependencyManager dependencyManager, Func<TItemIn, TItemOut> getter)
        {
            _dependencyManager = dependencyManager;
            _getter = getter;
        }

        private readonly DependencyManager _dependencyManager;

        private Func<TItemIn, TItemOut> _getter;
        private List<TItemOut> _list;

        public IEnumerable<TItemOut> Get(IReactiveCollectionConsumer<TItemOut> consumer)
        {
            _dependencyManager.RegisterDependency(consumer, this);
            return _list;
        }

        public ValueTask Added(TItemIn value)
        {
            var outValue = _getter(value);
            _list.Add(outValue);
            return _dependencyManager.ValueAdded(this, outValue);
        }
        public ValueTask Removed(TItemIn value)
        {
            var outValue = _getter(value);
            _list.Remove(outValue);
            return _dependencyManager.ValueRemoved(this, outValue);
        }
    }
}