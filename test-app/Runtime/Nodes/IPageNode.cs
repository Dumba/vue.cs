using System;

namespace test_app.Runtime.Nodes
{
    public interface IPageNode : IPageItem
    {
        Guid Id { get; }
    }
}