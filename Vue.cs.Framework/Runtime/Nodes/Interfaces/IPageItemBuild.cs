using System;
using System.Collections.Generic;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes.Interfaces
{
    public interface IPageItemBuild : IPageItem
    {
        List<IPageItem> InnerNodes { set; }
        
        void AddClass(string className);
        void AddAttribute(KeyValuePair<string, string?> attribute);
        void AddEventHandler(EventHandlerData eventHandler);
        void AddReactiveAttribute(IServiceProvider serviceProvider, string attributeName, IReactiveProvider<string?> valueProvider);
        void AddCondition(IReactiveProvider<bool> condition);

        void BuildCondition(IServiceProvider serviceProvider);
    }
}