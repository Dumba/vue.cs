using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using test_app.Generated;

namespace test_app.Components
{
    public abstract class BaseComponent : IDisposable
    {
        public Element Parent { get; set; }
        public IElement Body { get; protected set; }
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

        public void Render()
        {
            Body = BuildBody();
        }
        public abstract IElement BuildBody();

        public virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterBodyBuild()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}