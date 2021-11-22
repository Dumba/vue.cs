using System.Collections.Generic;
using System.Linq;

namespace test_app.Runtime.Nodes
{
    public class Template : IPageMultiItem
    {
        public Template(IEnumerable<IPageItem> items)
        {
            Items = items;
        }

        public IEnumerable<IPageItem> Items { get; }

        public IEnumerable<IPageNode> Nodes => Items.SelectMany(i => i.Nodes);
    }
}