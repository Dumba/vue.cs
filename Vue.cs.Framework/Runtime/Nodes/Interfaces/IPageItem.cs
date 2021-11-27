using System.Collections.Generic;
using Vue.cs.Framework.Runtime.Reactive;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageItem
    {
        /// <summary>
        ///   Return self or all direct nodes
        /// </summary>
        IEnumerable<IPageNode> Nodes { get; }
        
        /// <summary>
        ///   Return Nodes transformed for js
        /// </summary>
        IEnumerable<INodeBuilt> Build(DependencyManager dependencyManager, JsManipulator jsManipulator);

        /// <summary>
        ///   Reverse of Build - unregister dependency
        /// </summary>
        void Demolish();
    }
}