using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class NodeText : INode
    {
        public NodeText(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
        }

        public Guid Id { get; }
        public string Text { get; set; }
        
        public ValueTask RenderAsync(JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, Guid? insertBeforeNodeId = null)
        {
            return jsManipulator.InsertNode(parentElementId, this, insertBeforeNodeId);
        }
    }
}