using System.Collections.Generic;

namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
{
    public interface IReactiveCollectionProvider<TItem> : IReactiveCollectionProvider
    {
        IEnumerable<TItem> Get(IReactiveCollectionConsumer<TItem> consumer);
    }
    
    public interface IReactiveCollectionProvider
    {
    }
}