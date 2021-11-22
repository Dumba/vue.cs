using System.Collections.Generic;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.Data
{
    public class ReactiveCollection<TItem> : IReactiveCollectionProvider<TItem>
    {
        public ReactiveCollection(DependencyManager dependencyManager)
        {
            _dependencyManager = dependencyManager;
            _list = new List<TItem>();
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