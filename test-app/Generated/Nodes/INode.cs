using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public interface INode
    {
        Guid Id { get; }

        ValueTask RenderAsync(JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, Guid? insertBeforeNodeId = null);
    }
}