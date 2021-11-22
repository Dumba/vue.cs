using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public interface IPageItem
    {
        IEnumerable<IPageItem> GetNodes { get; }
    }
}