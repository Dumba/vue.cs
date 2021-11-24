using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Runtime.Reactive;

namespace test_app.Runtime.Nodes
{
    public interface IPageItem
    {
        /// <summary>
        ///   Return all direct nodes, recursively include template-nodes
        /// </summary>
        IEnumerable<IPageNode> Nodes { get; }
        bool IsVisible { get; }
    }

    public static class IPageItemExtension
    {
        public static async ValueTask Render(this IPageItem self, JsManipulator jsManipulator, Guid parentId)
        {
            foreach (var node in self.Nodes)
            {
                await jsManipulator.InsertNode(parentId, node.IsVisible ? node : new NodeComment(id: node.Id));

                if (node.IsVisible && node is INodeParent element)
                {
                    foreach (var child in element.Children)
                    {
                        await child.Render(jsManipulator, node.Id);
                    }
                }
            }
        }
    }
}