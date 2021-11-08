using System;
using test_app.Base;
using test_app.Generated.Reactive;
using test_app.Generated.Reactive.Visual;

namespace test_app.Generated.Nodes
{
    public class ComponentBuilder : INodeBuilder
    {
        public ComponentBuilder(DependencyManager dependencyManager, JsManipulator jsManipulator, BaseComponent component, Guid parentElementId)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            _component = component;
            _node = _component.BuildNodes(parentElementId);
        }

        private readonly DependencyManager _dependencyManager;
        private readonly JsManipulator _jsManipulator;

        private BaseComponent _component;
        private INodePositioned _node;
        private IReactiveProvider<bool> _condition;

        public ComponentBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;

            return this;
        }

        public INodePositioned Build()
        {
            if (_condition != null)
            {
                _node = new ReactiveNode(_dependencyManager, _node, _condition);
            }

            return _node;
        }
    }
}