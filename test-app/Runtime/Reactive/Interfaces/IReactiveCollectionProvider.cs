using System.Collections.Generic;

namespace test_app.Runtime.Reactive.Interfaces
{
    public interface IReactiveCollectionProvider<TItem> : IReactiveCollectionProvider
    {
        IEnumerable<TItem> Get(IReactiveCollectionConsumer<TItem> consumer);
    }
    
    public interface IReactiveCollectionProvider
    {
    }
}