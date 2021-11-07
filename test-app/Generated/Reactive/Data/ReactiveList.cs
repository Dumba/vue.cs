using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace test_app.Generated.Reactive.Data
{
    public class ReactiveList<TItem> : IReactiveProvider, IReactiveEnumerableProvider<TItem>
    {
        public ReactiveList(DependencyManager dependencyManager, IEnumerable<TItem> defaultValue = null)
        {
            _dependencyManager = dependencyManager;
            _list = defaultValue.ToList() ?? new List<TItem>();
        }

        private readonly DependencyManager _dependencyManager;
        private readonly List<TItem> _list;

        public void Add(TItem newValue)
        {
            _list.Add(newValue);
            _dependencyManager.ValueAdded(this, newValue);
        }

        public void Remove(TItem value)
        {
            _list.Remove(value);
            _dependencyManager.ValueRemoved(this, value);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}