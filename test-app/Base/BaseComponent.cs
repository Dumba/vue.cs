using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using test_app.Runtime.Nodes;
using test_app.Runtime.Nodes.Builders;
using test_app.Runtime.Reactive;

namespace test_app.Base
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
        private DotNetObjectReference<BaseComponent> _thisAsJsInterop;

        public ValueTask Render(Guid parentId)
        {
            var jsManipulator = _serviceProvider.GetService<JsManipulator>();
            
            var builder = new TemplateBuilder(_serviceProvider, this);
            Setup(builder);

            return builder
                .Build()
                .Render(jsManipulator, parentId);
        }

        public abstract void Setup(TemplateBuilder builder);

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}