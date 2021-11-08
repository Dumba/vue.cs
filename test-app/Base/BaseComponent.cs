using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using test_app.Generated;
using test_app.Generated.Nodes;

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

        protected ElementBuilder CreateRoot(Guid parentElementId, string tagName)
        {
            return new ElementBuilder(_serviceProvider, this, parentElementId, tagName);
        }

        public Task RenderAsync(Guid parentElementId, bool init)
        {
            var node = BuildNodes(parentElementId);
            return OnlyRenderAsync(node, init);
        }
        public INodePositioned BuildNodes(Guid parentElementId)
        {
            OnInitialize();
            return _buildNodes(parentElementId).Build();
        }
        public async Task OnlyRenderAsync(INodePositioned node, bool init)
        {
            await AfterBuildNodesAsync();
            await node.RenderAsync(_serviceProvider.GetService<JsManipulator>(), init);
            await AfterRenderAsync();
        }

        protected abstract INodeBuilder _buildNodes(Guid parentElementId);
        
        public virtual void OnInitialize()
        {
        }
        public virtual Task AfterBuildNodesAsync()
        {
            return Task.CompletedTask;
        }
        public virtual Task AfterRenderAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}