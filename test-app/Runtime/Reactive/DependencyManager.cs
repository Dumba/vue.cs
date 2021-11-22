using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive
{
    public class DependencyManager
    {
        public async ValueTask ValueChanged<TValue>(IReactiveProvider<TValue> master, TValue oldValue, TValue newValue)
            where TValue : class
        {
            if (oldValue == newValue)
                return;

            if (!_dependency.TryGetValue(master, out var slaves))
                return;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveConsumer<TValue>).Changed(oldValue, newValue));
                
            foreach (var task in tasks)
            {
                await task;
            }
        }

        public async ValueTask ValueAdded<TItem>(IReactiveCollectionProvider<TItem> master, TItem newValue)
        {
            if (!_enumerableDependency.TryGetValue(master, out var slaves))
                return;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveCollectionConsumer<TItem>).Added(newValue));
                
            foreach (var task in tasks)
            {
                await task;
            }
        }

        public async ValueTask ValueRemoved<TValue>(IReactiveCollectionProvider<TValue> master, TValue oldValue)
        {
            if (!_enumerableDependency.TryGetValue(master, out var slaves))
                return;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveCollectionConsumer<TValue>).Removed(oldValue));
            
            foreach (var task in tasks)
            {
                await task;
            }
        }

        public void RegisterDependency<TValue>(IReactiveConsumer<TValue> slave, params IReactiveProvider<TValue>[] masters)
        {
            foreach (var master in masters)
            {
                _dependency.TryAdd(master, new HashSet<IReactiveConsumer>());
                _dependency[master].Add(slave);
            }
        }
        public void RegisterDependency<TEnumerable>(IReactiveCollectionConsumer<TEnumerable> slave, params IReactiveCollectionProvider<TEnumerable>[] masters)
        {
            foreach (var master in masters)
            {
                _enumerableDependency.TryAdd(master, new HashSet<IReactiveCollectionConsumer>());
                _enumerableDependency[master].Add(slave);
            }
        }

        private Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>> _dependency = new Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>>();
        private Dictionary<IReactiveCollectionProvider, HashSet<IReactiveCollectionConsumer>> _enumerableDependency = new Dictionary<IReactiveCollectionProvider, HashSet<IReactiveCollectionConsumer>>();
    }
}