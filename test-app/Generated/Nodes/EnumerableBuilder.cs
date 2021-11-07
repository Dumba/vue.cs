using System.Threading.Tasks;
using test_app.Generated.Reactive;

namespace test_app.Generated.Nodes
{
    public class EnumerableBuilder<TItem> : IReactiveEnumerableConsumer<TItem>
    {
        public Task Added(TItem newValue)
        {
            throw new System.NotImplementedException();
        }

        public Task Removed(TItem oldValue)
        {
            throw new System.NotImplementedException();
        }
    }
}