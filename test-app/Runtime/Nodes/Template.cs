using System;
using System.Collections.Generic;
using System.Linq;
using test_app.Runtime.Nodes.Interfaces;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes
{
    public class Template : IPageMultiItem, IPageItemWithAttrs
    {
        public Template()
        {
            StartId = Guid.NewGuid();
            EndId = Guid.NewGuid();
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            Styles = new Dictionary<string, string>();
            EventHandlers = new List<EventHandlerData>();

            Items = new List<IPageItem>();
        }

        public Guid StartId { get; }
        public Guid EndId { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Styles { get; }
        public List<EventHandlerData> EventHandlers { get; }

        public List<IPageItem> Items { get; }
        public IReactiveProvider<bool> Condition { get; set; }

        public IEnumerable<IPageNode> Nodes => Items.SelectMany(i => i.Nodes)
            .Prepend(new NodeComment($" start template {StartId} / {EndId} ", StartId))
            .Append(new NodeComment($" end template {StartId} / {EndId} ", EndId));
        public bool IsVisible => Condition?.Get(null) != false;
    }
}