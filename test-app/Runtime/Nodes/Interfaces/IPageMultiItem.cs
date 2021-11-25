using System.Collections.Generic;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface IPageMultiItem : IPageItem
    {
        List<IPageItem> Items { get; }
    }
}