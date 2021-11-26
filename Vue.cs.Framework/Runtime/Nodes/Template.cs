using System;
using System.Collections.Generic;
using System.Linq;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class Template : IPageItemBuild
    {
        public Template(NodeComment startNode, NodeComment endNode)
        {
            StartNode = startNode;
            EndNode = endNode;

            InnerNodes = new List<IPageItem>();
        }

        public NodeComment StartNode { get; }
        public NodeComment EndNode { get; }

        public List<IPageItem> InnerNodes { get; set; }

        public IEnumerable<IPageNode> Nodes => InnerNodes.SelectMany(i => i.Nodes)
            .Prepend(StartNode)
            .Append(EndNode);

        public void AddClass(string className)
        {
            foreach (var node in InnerNodes)
            {
                if (node is IPageItemBuild element)
                {
                    element.AddClass(className);
                }
            }
        }
        public void AddAttribute(Attribute attribute)
        {
            foreach (var node in InnerNodes)
            {
                if (node is IPageItemBuild element)
                {
                    element.AddAttribute(attribute);
                }
            }
        }
        public void AddEventHandler(EventHandlerData eventHandler)
        {
            foreach (var node in InnerNodes)
            {
                if (node is IPageItemBuild element)
                {
                    element.AddEventHandler(eventHandler);
                }
            }
        }

        public void AddCondition(IReactiveProvider<bool> condition)
        {
            foreach (var node in InnerNodes)
            {
                if (node is IPageItemBuild element)
                {
                    element.AddCondition(condition);
                }
            }
        }

        public void BuildCondition(IServiceProvider serviceProvider)
        {
            foreach (var node in InnerNodes)
            {
                if (node is IPageItemBuild element)
                {
                    element.BuildCondition(serviceProvider);
                }
            }
        }
    }
}