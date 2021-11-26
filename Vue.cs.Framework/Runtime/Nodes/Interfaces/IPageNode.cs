using System;
using System.Text.Json;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageNode : IPageItem
    {
        Guid Id { get; }
        bool IsVisible { get; }
    }
}