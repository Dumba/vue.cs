using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class TextNodeBuilder : INodeBuilder
    {
        public TextNodeBuilder(string text)
        {
            Node = new TextNode(text);
        }

        public INode Node { get; }

        public Task InsertToDomAsync(JsManipulator jsManipulator, Guid parentId, BaseComponent parentComponent)
        {
            jsManipulator.InsertNode(parentId, Node);

            return Task.CompletedTask;
        }
    }
}