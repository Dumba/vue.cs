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

        public void RegisterDependency<TValue>(IReactiveConsumer<TValue> slave, params IReactiveProvider<TValue>[] masters)
        {
            foreach (var master in masters)
            {
                _dependency.TryAdd(master, new HashSet<IReactiveConsumer>());
                _dependency[master].Add(slave);
            }
        }

        private Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>> _dependency = new Dictionary<IReactiveProvider, HashSet<IReactiveConsumer>>();
    }
}