using System.Collections.Generic;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageItemCollection : IPageItem
    {
        List<IPageItem> InnerNodes { set; }
    }
}