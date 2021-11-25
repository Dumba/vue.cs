using System;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface IPageNode : IPageItem
    {
        Guid Id { get; }
    }
}