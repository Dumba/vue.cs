using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using test_app.Generated.Nodes;

namespace test_app.Base
{
    public abstract class BaseComponent : IDisposable
    {
        public BaseComponent Parent { get; set; }
        public INode Body { get; protected set; }
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

        public async Task RenderAsync(Guid parentId)
        {
            await OnInitializeAsync();
            Body = await BuildBody(parentId);
            await AfterBodyBuildAsync();
        }

        protected abstract Task<INode> BuildBody(Guid parentId);
        protected virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }
        protected virtual Task AfterBodyBuildAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}