using System;

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

        public object Serialize()
        {
            return this;
        }
    }
}