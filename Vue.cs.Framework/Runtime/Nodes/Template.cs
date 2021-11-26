using System.Collections.Generic;
using System.Linq;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class Template : IPageItem, IPageItemCollection
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
    }
}