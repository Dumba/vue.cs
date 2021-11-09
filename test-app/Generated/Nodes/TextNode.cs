using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class TextNode : INode
    {
        public TextNode(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
        }

        public Guid Id { get; }
        public string Text { get; set; }
        
        public Task RenderAsync(JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, Guid? insertBeforeNodeId = null)
        {
            jsManipulator.InsertNode(parentElementId, this, insertBeforeNodeId);

            return Task.CompletedTask;
        }
    }
}