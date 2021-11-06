using System;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated.Reactive;

namespace test_app.Generated.Nodes
{
    public class ComponentToElementBuilder : INodeBuilder, IReactiveConsumer<bool>
    {
        public ComponentToElementBuilder(DependencyManager dependencyManager, JsManipulator jsManipulator, BaseComponent component, Guid parentElementId)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            _component = component;
            _parentElementId = parentElementId;
        }
        
        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;

        private BaseComponent _component;
        private Guid _parentElementId;
        private IReactiveProvider<bool> _condition;

        public INode Node => _component.Body;
        public bool IsOnPage => true;
        public INodeBuilder NextNodeBuilder { get; set; }

        public ComponentToElementBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;
            _dependencyManager.RegisterDependency(this, condition);

            return this;
        }

        public Task InsertToDomAsync(Guid? insertBeforeNodeId = null)
        {
            // v-if
            if (_condition?.Get() == false)
                return Task.CompletedTask;
            
            return _component.RenderAsync(_parentElementId, insertBeforeNodeId);
        }

        public Task Changed(bool oldValue, bool newValue)
        {
            // no change - ignore
            if (oldValue == newValue)
            {
                return Task.CompletedTask;
            }

            // insert
            if (newValue)
            {
                return InsertToDomAsync(NextNodeBuilder.Node.Id);
            }
            // remove
            else
            {
                _jsManipulator.RemoveNode(Node.Id);
                return Task.CompletedTask;
            }
        }
    }
}