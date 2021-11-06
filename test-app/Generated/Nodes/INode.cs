using System;

namespace test_app.Generated.Nodes
{
    public interface INode
    {
        Guid Id { get; }
        object Serialize();
    }
}