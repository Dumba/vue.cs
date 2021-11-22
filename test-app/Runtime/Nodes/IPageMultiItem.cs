using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public interface IPageMultiItem : IPageItem
    {
        List<IPageItem> Items { get; }
    }
}