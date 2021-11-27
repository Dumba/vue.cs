using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Vue.cs.Framework.Extensions;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Builders;
using Vue.cs.Framework.Runtime.Reactive;

namespace Vue.cs.Framework.Base
{
    public abstract class BaseComponent : IDisposable
    {
        public BaseComponent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected readonly IServiceProvider _serviceProvider;

        public DotNetObjectReference<BaseComponent> ThisAsJsInterop
        {
            get
            {
                if (_thisAsJsInterop == null)
                {
                    _thisAsJsInterop = DotNetObjectReference.Create<BaseComponent>(this);
                }

                return _thisAsJsInterop;
            }
        }
        private DotNetObjectReference<BaseComponent>? _thisAsJsInterop;

        public ValueTask Render(string parentElementSelector)
        {
            var dependencyManager = _serviceProvider.Get<DependencyManager>();
            var jsManipulator = _serviceProvider.Get<JsManipulator>();
            
            var builder = new TemplateBuilder(_serviceProvider, this, $"component {GetType().Name}");
            Setup(builder);

            var pageItem = builder.Build();
            return jsManipulator.InsertNode(parentElementSelector, pageItem);
        }

        public abstract void Setup(Builder builder, IEnumerable<IPageItem>? childNodes = null);

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}