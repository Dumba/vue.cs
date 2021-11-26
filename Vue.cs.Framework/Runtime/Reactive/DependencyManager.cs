using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive
{
    public class DependencyManager
    {
        public async ValueTask ValueChanged<TValue>(IReactiveProvider<TValue> master, TValue? oldValue, TValue? newValue)
        {
            if (oldValue?.Equals(newValue) ?? newValue is null)
                return;

            if (!_dependency.TryGetValue(master, out var slaves))
                return;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveConsumer<TValue>)!.Changed(oldValue, newValue));
                
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
                .Select(slave => (slave as IReactiveCollectionConsumer<TItem>)!.Added(newValue));
                
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
                .Select(slave => (slave as IReactiveCollectionConsumer<TValue>)!.Removed(oldValue));
            
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
        public void UnregisterDependency<TValue>(IReactiveConsumer<TValue> slave, params IReactiveProvider<TValue>[] masters)
        {
            foreach (var master in masters)
            {
                if (_dependency.TryGetValue(master, out var slaves))
                    slaves.Remove(slave);
            }
        }
        public void RegisterDependency<TItem>(IReactiveCollectionConsumer<TItem> slave, params IReactiveCollectionProvider<TItem>[] masters)
        {
            foreach (var master in masters)
            {
                _enumerableDependency.TryAdd(master, new HashSet<IReactiveCollectionConsumer>());
                _enumerableDependency[master].Add(slave);
            }
        }
        public void UnregisterDependency<TItem>(IReactiveCollectionConsumer<TItem> slave, params IReactiveCollectionProvider<TItem>[] masters)
        {
            foreach (var master in masters)
            {
                if (_enumerableDependency.TryGetValue(master, out var slaves))
                {
                    slaves.Remove(slave);
                }
            }
        }

        private Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>> _dependency = new Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>>();
        private Dictionary<IReactiveCollectionProvider, HashSet<IReactiveCollectionConsumer>> _enumerableDependency = new Dictionary<IReactiveCollectionProvider, HashSet<IReactiveCollectionConsumer>>();
    }
}