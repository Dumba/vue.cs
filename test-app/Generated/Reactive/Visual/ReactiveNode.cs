using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test_app.Generated.Nodes;

namespace test_app.Generated.Reactive.Visual
{
    public class ReactiveNode : IReactiveConsumer<bool>, INodePositioned
    {
        public ReactiveNode(DependencyManager dependencyManager, INodePositioned nodePositioned, IReactiveProvider<bool> condition)
        {
            _nodePositioned = nodePositioned;
            _condition = condition;

            dependencyManager.RegisterDependency(this, condition);
        }

        private JsManipulator _jsManipulator;
        private INodePositioned _nodePositioned;
        private IReactiveProvider<bool> _condition;

        public Guid ParentElementId => _nodePositioned.ParentElementId;
        public INode Node => _nodePositioned.Node;

        public List<INodePositioned> Children => _nodePositioned.Children;
        public INodePositioned PrevNode
        {
            get => _nodePositioned.PrevNode;
            set => _nodePositioned.PrevNode = value;
        }
        public INodePositioned NextNode
        {
            get => _nodePositioned.NextNode;
            set => _nodePositioned.NextNode = value;
        }
        public INodePositioned GetAsNextVisibleNode => _condition.Get() ? this : NextNode?.GetAsNextVisibleNode;

        public ValueTask RenderAsync(JsManipulator jsManipulator, bool init)
        {
            _jsManipulator = jsManipulator;

            if (_condition.Get())
                return _nodePositioned.RenderAsync(jsManipulator, init);

            // ignore
            return ValueTask.CompletedTask;
        }

        public ValueTask Changed(bool oldValue, bool newValue)
        {
            // doesn't change
            if (oldValue == newValue)
                return ValueTask.CompletedTask;

            // add
            if (newValue)
            {
                return _nodePositioned.RenderAsync(_jsManipulator, false);
            }
            
            // remove
            return _jsManipulator.RemoveNode(Node.Id);
        }

        public class Builder
        {
            public Builder(DependencyManager dependencyManager)
            {
                _dependencyManager = dependencyManager;
            }

            private readonly DependencyManager _dependencyManager;
            
            public ReactiveNode Build(INodePositioned nodePositioned, IReactiveProvider<bool> condition)
            {
                return new ReactiveNode(_dependencyManager, nodePositioned, condition);
            }
        }
    }
}