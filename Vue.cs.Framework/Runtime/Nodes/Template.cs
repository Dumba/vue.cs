using System.Collections.Generic;
using System.Linq;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Reactive;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class Template : IPageItem, IPageItemCollection
    {
        public Template(NodeComment startNode, NodeComment endNode)
        {
            _startNode = startNode;
            _endNode = endNode;

            InnerNodes = new List<IPageItem>();
        }
        
        private NodeComment _startNode;
        private NodeComment _endNode;

        public List<IPageItem> InnerNodes { get; set; }
        public IEnumerable<IPageNode> Nodes => InnerNodes
            .SelectMany(i => i.Nodes)
            .Prepend(_startNode)
            .Append(_endNode);

        public IEnumerable<INodeBuilt> Build(DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            return _startNode.Build(dependencyManager, jsManipulator)
                .Concat(InnerNodes.SelectMany(i => i.Build(dependencyManager, jsManipulator)))
                .Concat(_endNode.Build(dependencyManager, jsManipulator));
        }

        public void Demolish()
        {
            _startNode.Demolish();
            _endNode.Demolish();
            
            foreach (var item in InnerNodes)
            {
                item.Demolish();
            }
        }
    }
}