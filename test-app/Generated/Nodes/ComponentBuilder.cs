using System;
using Microsoft.Extensions.DependencyInjection;
using test_app.Base;
using test_app.Generated.Reactive;
using test_app.Generated.Reactive.Visual;

namespace test_app.Generated.Nodes
{
    public class ComponentBuilder<TComponent> : INodeBuilder where TComponent : BaseComponent
    {
        public ComponentBuilder(IServiceProvider serviceProvider, Guid parentElementId)
        {
            _serviceProvider = serviceProvider;

            _component = serviceProvider.GetService<TComponent>();
            _node = _component.BuildNodes(parentElementId);
        }

        private readonly IServiceProvider _serviceProvider;

        private BaseComponent _component;
        private INodePositioned _node;
        private IReactiveProvider<bool> _condition;

        public ComponentBuilder<TComponent> SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;

            return this;
        }

        public INodePositioned Build()
        {
            if (_condition != null)
            {
                _node = _serviceProvider.GetService<ReactiveNode.Builder>().Build(_node, _condition);
            }

            return _node;
        }
    }
}