using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class NodePositioned : INodePositioned
    {
        public NodePositioned(INode node, BaseComponent parentComponent, Guid parentElementId)
        {
            Node = node;
            ParentComponent = parentComponent;
            ParentElementId = parentElementId;
            Children = new List<INodePositioned>();
        }

        public INode Node { get; }
        public BaseComponent ParentComponent { get; }
        public Guid ParentElementId { get; }
        public List<INodePositioned> Children { get; }

        public INodePositioned PrevNode { get; set; }
        public INodePositioned NextNode { get; set; }
        public INodePositioned GetAsNextVisibleNode => this;

        public async Task RenderAsync(JsManipulator jsManipulator, bool init)
        {
            await Node.RenderAsync(jsManipulator, ParentComponent, ParentElementId, init ? null : NextNode?.GetAsNextVisibleNode?.Node.Id);
            
            foreach (var child in Children)
            {
                await child.RenderAsync(jsManipulator, true);
            }
        }
    }
}