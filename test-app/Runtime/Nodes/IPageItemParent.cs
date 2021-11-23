using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public interface INodeParent : IPageItem
    {
        List<IPageItem> Children { get; }
    }
}