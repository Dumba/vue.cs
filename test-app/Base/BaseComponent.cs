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

        public async Task RenderAsync(Guid parentId, Guid? insertBeforeNodeId = null)
        {
            await OnInitializeAsync();
            Body = await BuildBody(parentId, insertBeforeNodeId);
            await AfterBodyBuildAsync();
        }
        protected abstract Task<INode> BuildBody(Guid parentId, Guid? insertBeforeNodeId = null);
        protected virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }
        protected virtual Task AfterBodyBuildAsync()
        {
            return Task.CompletedTask;
        }

        protected ElementBuilder CreateRoot(Guid parentElementId, string tagName)
        {
            return new ElementBuilder(_dependencyManager, _jsManipulator, this, parentElementId, tagName);
        }

        public void Dispose()
        {
            _thisAsJsInterop?.Dispose();
        }
    }
}