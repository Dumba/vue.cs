using System;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageNode : IPageItem
    {
        Guid Id { get; }
    }
}