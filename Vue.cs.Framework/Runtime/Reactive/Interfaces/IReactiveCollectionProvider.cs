using System.Collections.Generic;

namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
{
    public interface IReactiveCollectionProvider<TItem> : IReactiveCollectionProvider
    {
        IEnumerable<TItem> Value { get; }
    }

    public interface IReactiveCollectionProvider
    {
    }
}