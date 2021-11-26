using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeText : IPageNode
    {
        public NodeText(string text, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Text = text;
        }

        public Guid Id { get; }
        public string Text { get; }

        [JsonIgnore]
        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        [JsonIgnore]
        public bool IsVisible => true;
    }
}