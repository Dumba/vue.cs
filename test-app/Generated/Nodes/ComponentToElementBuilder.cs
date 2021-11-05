using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class ComponentToElementBuilder : INodeBuilder
    {
        public ComponentToElementBuilder(BaseComponent component)
        {
            _component = component;
        }

        private BaseComponent _component;

        public INode Node => throw new System.NotImplementedException();

        public Task InsertToDomAsync(JsManipulator jsManipulator, Guid parentId, BaseComponent parentComponent)
        {
            return _component.RenderAsync(parentId);
        }
    }
}