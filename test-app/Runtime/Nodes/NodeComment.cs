using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using test_app.Runtime.Nodes.Interfaces;

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

        [JsonIgnore]
        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        [JsonIgnore]
        public bool IsVisible => true;
    }
}