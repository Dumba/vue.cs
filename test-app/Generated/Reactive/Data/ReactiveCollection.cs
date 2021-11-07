using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace test_app.Generated.Reactive
{
    public class ReactiveCollection<TCollection, TItem> : IReactiveProvider<TCollection>, IEnumerable<TItem>
        where TCollection : IEnumerable<TItem>
    {
        public ReactiveCollection(DependencyManager dependencyManager, TCollection value = default)
        {
            _dependencyManager = dependencyManager;
            _value = value;
        }

        private readonly DependencyManager _dependencyManager;
        private TCollection _value;

        public TCollection Get()
        {
            return _value;
        }

        public Task Set(TCollection newValue)
        {
            var oldValue = _value;
            _value = newValue;
            return _dependencyManager.ValueChanged(this, oldValue, newValue);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_value as IEnumerable).GetEnumerator();
        }
    }
}