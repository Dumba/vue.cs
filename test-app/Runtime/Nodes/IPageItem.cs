using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public interface IPageItem
    {
        /// <summary>
        ///   Return all direct nodes, recursively include template-nodes
        /// </summary>
        IEnumerable<IPageNode> Nodes { get; }
    }
}