using System;
using Microsoft.Extensions.DependencyInjection;
using test_app.Base;
using test_app.Runtime.Reactive.Interfaces;
using test_app.Runtime.Reactive.PageItems;

namespace test_app.Runtime.Nodes.Builders
{
    public class TemplateBuilder : IBuilder
    {
        public TemplateBuilder(IServiceProvider serviceProvider, BaseComponent parentComponent)
        {
            _serviceProvider = serviceProvider;
            _parentComponent = parentComponent;
            _template = new Template();
        }

        private readonly IServiceProvider _serviceProvider;
        private BaseComponent _parentComponent;
        private Template _template;
        private IReactiveProvider<bool> _condition;
        

        public TemplateBuilder AddClass(string className)
        {
            _template.Classes.Add(className);

            return this;
        }
        public TemplateBuilder AddAttribute(string name, string value)
        {
            _template.Attributes.Add(name, value);

            return this;
        }
        // public TemplateBuilder AddAttribute(string name, IReactiveProvider<string> valueProvider)
        // {
        //     var reactiveAttribute = _serviceProvider.GetService<ReactiveAttribute.Builder>()
        //         .Build(_element.Id, name, valueProvider, out var text);
        //     _template.Attributes.Add(name, text);

        //     return this;
        // }
        public TemplateBuilder AddEventListener(string eventName, string methodName, params object[] @params)
        {
            _template.EventHandlers.Add((eventName, methodName, @params));

            return this;
        }

        public TemplateBuilder SetCondition(IReactiveProvider<bool> condition)
        {
            _condition = condition;
            return this;
        }

        public TemplateBuilder AddText(string text)
        {
            var child = new NodeText(text);

            _addChild(child);

            return this;
        }
        // public TemplateBuilder AddText(IReactiveProvider<string> textProvider)
        // {
        //     var reactiveText = _serviceProvider.GetService<ReactiveText.Builder>()
        //         .Build(_element.Id, textProvider, out var text);
        //     var child = new NodeText(text);
        //     _addChild(child);

        //     return this;
        // }

        public TemplateBuilder AddChild(string tagName, Action<ElementBuilder> setupChild = null)
        {
            var childBuilder = new ElementBuilder(_serviceProvider, _parentComponent, tagName);
            if (setupChild != null)
                setupChild(childBuilder);

            _addChild(childBuilder.Build());

            return this;
        }

        private void _addChild(IPageItem child)
        {
            _template.Items.Add(child);
        }

        public IPageItem Build()
        {
            if (_condition != null)
            {
                var reactiveNode = _serviceProvider.GetService<ReactivePageItem.Builder>()
                    .Build(_template, _condition, out bool visible);

                if (visible == false)
                    return new NodeComment(id: _template.EndId);
            }

            return _template;
        }
    }
}