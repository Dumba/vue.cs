using System;
using System.Collections.Generic;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface IPageItemBuild : IPageItem
    {
        List<IPageItem> InnerNodes { set; }
        IReactiveProvider<bool> Condition { set; }
        
        void AddClass(string className);
        void AddAttribute(KeyValuePair<string, string> attribute);
        void AddEventHandler(EventHandlerData eventHandler);
        void AddReactiveAttribute(IServiceProvider serviceProvider, string attributeName, IReactiveProvider<string> valueProvider);
    }
}