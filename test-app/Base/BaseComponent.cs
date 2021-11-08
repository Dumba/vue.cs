using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using test_app.Generated;
using test_app.Generated.Nodes;
using test_app.Generated.Reactive;

namespace test_app.Base
{
    public abstract class BaseComponent : IDisposable
    {
        public BaseComponent(DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;
        }

        protected readonly DependencyManager _dependencyManager;
        protected readonly JsManipulator _jsManipulator;

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
            return new ElementBuilder(_dependencyManager, _jsManipulator, this, parentElementId, tagName);
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
            await node.RenderAsync(_jsManipulator, init);
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