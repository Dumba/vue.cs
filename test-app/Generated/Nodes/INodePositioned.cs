using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace test_app.Generated.Nodes
{
    public interface INodePositioned
    {
        INode Node { get; }
        Guid ParentElementId { get; }
        List<INodePositioned> Children { get; }
        INodePositioned PrevNode { get; set; }
        INodePositioned NextNode { get; set; }
        INodePositioned GetAsNextVisibleNode { get; }

        Task RenderAsync(JsManipulator jsManipulator, bool init);
    }
}