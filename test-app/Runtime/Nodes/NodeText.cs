using System;
using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public class NodeText : IPageNode
    {
        public NodeText(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
        }

        public Guid Id { get; }
        public string Text { get; }

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
    }
}