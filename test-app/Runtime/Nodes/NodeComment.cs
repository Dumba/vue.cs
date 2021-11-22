using System;
using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public class NodeComment : IPageNode
    {
        public NodeComment(string content = null, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Content = content ?? $" placeholder for {Id} ";
        }

        public Guid Id { get; }
        public string Content { get; }

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
    }
}