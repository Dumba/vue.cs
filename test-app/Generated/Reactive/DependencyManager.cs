using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.Generated.Reactive
{
    public class DependencyManager
    {
        public Task ValueChanged<TValue>(IReactiveProvider<TValue> master, TValue oldValue, TValue newValue)
        {
            if (!_dependency.TryGetValue(master, out var slaves))
                return Task.CompletedTask;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveConsumer<TValue>).Changed(oldValue, newValue));
                
            return Task.WhenAll(tasks);
        }

        public Task ValueAdded<TValue>(IReactiveEnumerableProvider<TValue> master, TValue newValue)
        {
            if (!_enumerableDependency.TryGetValue(master, out var slaves))
                return Task.CompletedTask;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveEnumerableConsumer<TValue>).Added(newValue));
                
            return Task.WhenAll(tasks);
        }

        public Task ValueRemoved<TValue>(IReactiveEnumerableProvider<TValue> master, TValue oldValue)
        {
            if (!_enumerableDependency.TryGetValue(master, out var slaves))
                return Task.CompletedTask;
                
            var tasks = slaves
                .Select(slave => (slave as IReactiveEnumerableConsumer<TValue>).Removed(oldValue));
                
            return Task.WhenAll(tasks);
        }

        public void RegisterDependency<TValue>(IReactiveConsumer<TValue> slave, params IReactiveProvider<TValue>[] masters)
        {
            foreach (var master in masters)
            {
                _dependency.TryAdd(master, new HashSet<IReactiveConsumer>());
                _dependency[master].Add(slave);
            }
        }
        public void RegisterDependency<TEnumerable>(IReactiveEnumerableConsumer<TEnumerable> slave, params IReactiveEnumerableProvider<TEnumerable>[] masters)
        {
            foreach (var master in masters)
            {
                _enumerableDependency.TryAdd(master, new HashSet<IReactiveEnumerableConsumer>());
                _enumerableDependency[master].Add(slave);
            }
        }

        private Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>> _dependency = new Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>>();
        private Dictionary<IReactiveEnumerableProvider, HashSet<IReactiveEnumerableConsumer>> _enumerableDependency = new Dictionary<IReactiveEnumerableProvider, HashSet<IReactiveEnumerableConsumer>>();
    }
}