using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public interface INodeBuilder
    {
        INode Node { get; }

        Task InsertToDomAsync(JsManipulator jsManipulator, Guid parentId, BaseComponent parentComponent);
    }
}