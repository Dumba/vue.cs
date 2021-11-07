using System.Threading.Tasks;

namespace test_app.Generated.Reactive
{
    public interface IReactiveEnumerableConsumer<TItem> : IReactiveEnumerableConsumer
    {
        Task Added(TItem newValue);
        Task Removed(TItem oldValue);
    }

    public interface IReactiveEnumerableConsumer
    {
    }
}