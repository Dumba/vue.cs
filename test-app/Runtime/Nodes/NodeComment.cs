using System;
using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public class NodeComment : IPageNode
    {
        public NodeComment(string content)
        {
            Id = Guid.NewGuid();
            Content = content;
        }

        public Guid Id { get; }
        public string Content { get; }

        public IEnumerable<IPageItem> GetNodes { get { yield return this; } }
    }
}