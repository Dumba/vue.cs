using System;
using System.Collections.Generic;
using System.Linq;
using test_app.Runtime.Nodes.Interfaces;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes
{
    public class Template : IPageItemBuild
    {
        public Template(NodeComment startNode, NodeComment endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            EventHandlers = new HashSet<EventHandlerData>();

            InnerNodes = new List<IPageItem>();
        }

        public NodeComment StartNode { get; }
        public NodeComment EndNode { get; }
        public List<string> Classes { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public HashSet<EventHandlerData> EventHandlers { get; set; }

        public List<IPageItem> InnerNodes { get; set; }
        public IReactiveProvider<bool> Condition { get; set; }

        public IEnumerable<IPageNode> Nodes => InnerNodes.SelectMany(i => i.Nodes)
            .Prepend(StartNode)
            .Append(EndNode);
        public bool IsVisible => Condition?.Get(null) ?? true;


        public void AddReactiveAttribute(IServiceProvider serviceProvider, string attributeName, IReactiveProvider<string> valueProvider)
        {
            foreach (var item in InnerNodes)
            {
                if (item is IPageItemBuild pageItem)
                {
                    pageItem.AddReactiveAttribute(serviceProvider, attributeName, valueProvider);
                }
            }
        }
    }
}