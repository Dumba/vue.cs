using System.Collections.Generic;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface INodeParent : IPageItem
    {
        List<IPageItem> Children { get; }
    }
}