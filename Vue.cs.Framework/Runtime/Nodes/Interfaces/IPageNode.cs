using System;
using Vue.cs.Framework.Runtime.Reactive;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageNode : IPageItem
    {
        Guid Id { get; }
        bool IsVisible { get; }
        
        object Build(DependencyManager dependencyManager, JsManipulator jsManipulator);
        void Demolish();
    }
}