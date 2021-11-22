using System.Threading.Tasks;

namespace test_app.Runtime.Reactive.Interfaces
{
    public interface IReactiveCollectionConsumer<TItem> : IReactiveCollectionConsumer
    {
        ValueTask Added(TItem value);
        ValueTask Removed(TItem value);
    }
    
    public interface IReactiveCollectionConsumer
    {
    }
}