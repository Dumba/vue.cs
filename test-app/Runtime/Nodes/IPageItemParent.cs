using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public interface INodeParent
    {
        List<IPageItem> Children { get; }
    }
}