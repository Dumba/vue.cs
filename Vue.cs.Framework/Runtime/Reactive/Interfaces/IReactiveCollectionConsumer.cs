using System.Threading.Tasks;

namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
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