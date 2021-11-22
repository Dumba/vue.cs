using System;
using System.Collections.Generic;
using System.Linq;

namespace test_app.Runtime.Nodes
{
    public class Template : IPageMultiItem
    {
        public Template()
        {
            StartId = Guid.NewGuid();
            EndId = Guid.NewGuid();
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            Styles = new Dictionary<string, string>();
            EventHandlers = new List<(string @event, string componentMethod, object[] @params)>();

            Items = new List<IPageItem>();
        }

        public Guid StartId { get; }
        public Guid EndId { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Styles { get; }
        public List<(string @event, string componentMethod, object[] @params)> EventHandlers { get; }

        public List<IPageItem> Items { get; }

        public IEnumerable<IPageNode> Nodes => Items.SelectMany(i => i.Nodes);
    }
}