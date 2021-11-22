using System.Collections.Generic;

namespace test_app.Runtime.Nodes
{
    public class Template : IPageMultiItem
    {
        public Template(IEnumerable<IPageItem> items)
        {
            Items = items;
        }

        public IEnumerable<IPageItem> Items { get; }

        public IEnumerable<IPageItem> GetNodes => Items;
    }
}