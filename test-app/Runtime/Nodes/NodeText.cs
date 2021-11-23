using System;
using System.Collections.Generic;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes
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
        public IReactiveProvider<bool> Condition { get; set; }

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        public bool IsVisible => Condition?.Get(null) != false;
    }
}