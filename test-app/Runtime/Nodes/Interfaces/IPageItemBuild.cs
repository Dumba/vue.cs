using System;
using System.Collections.Generic;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface IPageItemBuild : IPageItem
    {
        List<string> Classes { get; set; }
        Dictionary<string, string> Attributes { get; set; }
        HashSet<EventHandlerData> EventHandlers { get; set; }
        IReactiveProvider<bool> Condition { get; set; }
        
        List<IPageItem> InnerNodes { get; set; }

        void AddReactiveAttribute(IServiceProvider serviceProvider, string attributeName, IReactiveProvider<string> valueProvider);
    }
}