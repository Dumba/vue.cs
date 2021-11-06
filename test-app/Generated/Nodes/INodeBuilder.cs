using System;
using System.Threading.Tasks;

namespace test_app.Generated.Nodes
{
    public interface INodeBuilder
    {
        INode Node { get; }
        bool IsOnPage { get; }
        INodeBuilder NextNodeBuilder { get; set; }

        Task InsertToDomAsync(Guid? insertBeforeNodeId = null);
    }
}