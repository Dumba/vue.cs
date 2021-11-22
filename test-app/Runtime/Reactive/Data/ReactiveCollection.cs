using System.Collections.Generic;
using test_app.Runtime.Reactive;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.ReactiveData
{
    public class ReactiveCollection<TItem> : IReactiveCollectionProvider<TItem>
    {
        public ReactiveCollection(DependencyManager dependencyManager)
        {
            _dependencyManager = dependencyManager;
        }

        private readonly DependencyManager _dependencyManager;
        private List<TItem> _list;

        public IEnumerable<TItem> Get(IReactiveCollectionConsumer<TItem> consumer)
        {
            _dependencyManager.RegisterDependency(consumer, this);
            return _list;
        }
    }
}