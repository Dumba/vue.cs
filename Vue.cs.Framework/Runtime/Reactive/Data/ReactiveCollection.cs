using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive.Data
{
    public class ReactiveCollection<TItem> : IReactiveCollectionProvider<TItem>
    {
        public ReactiveCollection(DependencyManager dependencyManager, IEnumerable<TItem>? initCollection = null)
        {
            _dependencyManager = dependencyManager;
            _list = initCollection?.ToList() ?? new List<TItem>();
        }

        private readonly DependencyManager _dependencyManager;
        private List<TItem> _list;

        public IEnumerable<TItem> Get(IReactiveCollectionConsumer<TItem> consumer)
        {
            _dependencyManager.RegisterDependency(consumer, this);
            return _list;
        }

        public ValueTask Add(TItem newValue)
        {
            _list.Add(newValue);
            return _dependencyManager.ValueAdded(this, newValue);
        }

        public ValueTask Remove(TItem value)
        {
            _list.Remove(value);
            return _dependencyManager.ValueRemoved(this, value);
        }
    }
}