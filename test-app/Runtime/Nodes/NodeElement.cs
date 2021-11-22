using System;
using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public class NodeElement : IPageNode, INodeParent
    {
        public NodeElement(string tagName)
        {
            Id = Guid.NewGuid();
            TagName = tagName;
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            Styles = new Dictionary<string, string>();
            EventHandlers = new List<(string @event, string componentMethod, object[] @params)>();

            Children = new List<IPageItem>();
        }

        public Guid Id { get; }
        public string TagName { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Styles { get; }
        public List<(string @event, string componentMethod, object[] @params)> EventHandlers { get; }

        public List<IPageItem> Children { get; }

        public IEnumerable<IPageItem> GetNodes { get { yield return this; } }
    }
}