using System.Collections.Generic;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface INodeParent : IPageItem
    {
        List<IPageItem> Children { get; }
    }
}