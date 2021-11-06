using System;
using System.Threading.Tasks;

namespace test_app.Generated.Nodes
{
    public class TextNodeBuilder : INodeBuilder
    {
        public TextNodeBuilder(JsManipulator jsManipulator, Guid parentElementId, string text)
        {
            _jsManipulator = jsManipulator;
            _parentElementId = parentElementId;

            Node = new TextNode(text);
        }

        private readonly JsManipulator _jsManipulator;
        private readonly Guid _parentElementId;

        public INode Node { get; }
        public bool IsOnPage => true;
        public INodeBuilder NextNodeBuilder { get; set; }

        public Task InsertToDomAsync(Guid? insertBeforeNodeId = null)
        {
            _jsManipulator.InsertNode(_parentElementId, Node, insertBeforeNodeId);

            return Task.CompletedTask;
        }
    }
}