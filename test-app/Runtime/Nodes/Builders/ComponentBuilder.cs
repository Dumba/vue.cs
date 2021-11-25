using System;
using Microsoft.Extensions.DependencyInjection;
using test_app.Base;
using test_app.Runtime.Nodes.Interfaces;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes.Builders
{
    public class ComponentBuilder<TComponent> : IBuilder where TComponent : BaseComponent
    {
        public ComponentBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _component = serviceProvider.GetService<TComponent>();
        }

        private readonly IServiceProvider _serviceProvider;

        private BaseComponent _component;
        private IReactiveProvider<bool> _condition;

        public ComponentBuilder<TComponent> SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;

            return this;
        }

        public IPageItem Build()
        {
            var builder = new TemplateBuilder(_serviceProvider, _component);
            _component.Setup(builder);

            if (_condition != null)
            {
                builder.SetCondition(_condition);
            }

            return builder.Build();
        }
    }
}
