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
        void AddAttribute(Attribute attribute);
        void AddEventHandler(EventHandlerData eventHandler);
        void AddCondition(IReactiveProvider<bool> condition);

        void BuildCondition(IServiceProvider serviceProvider);
    }
}