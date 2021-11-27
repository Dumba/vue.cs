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

    // public static class IPageItemExtension
    // {
    //     public static async ValueTask Render(this IPageItem self, JsManipulator jsManipulator, Guid parentId)
    //     {
    //         foreach (var node in self.Nodes)
    //         {
    //             await jsManipulator.InsertNode(parentId, node.IsVisible ? node : new NodeComment(id: node.Id));

    //             if (node.IsVisible && node is INodeParent element)
    //             {
    //                 foreach (var child in element.Children)
    //                 {
    //                     await child.Render(jsManipulator, node.Id);
    //                 }
    //             }
    //         }
    //     }

    //     public static async ValueTask Render(this IPageItem self, JsManipulator jsManipulator, string parentElementSelector)
    //     {
    //         foreach (var node in self.Nodes)
    //         {
    //             await jsManipulator.InsertNode(parentElementSelector, node.IsVisible ? node : new NodeComment(id: node.Id));

    //             if (node.IsVisible && node is INodeParent element)
    //             {
    //                 foreach (var child in element.Children)
    //                 {
    //                     await child.Render(jsManipulator, node.Id);
    //                 }
    //             }
    //         }
    //     }
    // }
}