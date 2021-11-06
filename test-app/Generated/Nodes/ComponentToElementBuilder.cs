using System;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class ComponentToElementBuilder : INodeBuilder
    {
        public ComponentToElementBuilder(BaseComponent component, Guid parentElementId)
        {
            _component = component;
            _parentElementId = parentElementId;
        }

        private BaseComponent _component;
        private Guid _parentElementId;

        public INode Node => _component.Body;
        public bool IsOnPage => true;
        public INodeBuilder NextNodeBuilder { get; set; }

        public Task InsertToDomAsync(Guid? insertBeforeNodeId = null)
        {
            return _component.RenderAsync(_parentElementId, insertBeforeNodeId);
        }
    }
}