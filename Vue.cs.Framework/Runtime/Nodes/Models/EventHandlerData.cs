using Microsoft.JSInterop;
using Vue.cs.Framework.Base;

namespace Vue.cs.Framework.Runtime.Nodes.Models
{
    public class EventHandlerData
    {
        public EventHandlerData(string @event, DotNetObjectReference<BaseComponent> component, string componentMethodName, params object[] @params)
        {
            Event = @event;
            ComponentInterop = component;
            ComponentMethodName = componentMethodName;
            Params = @params;
        }

        public string Event { get; set; }
        public DotNetObjectReference<BaseComponent> ComponentInterop { get; set; }
        public string ComponentMethodName { get; set; }
        public object[] Params { get; set; }
    }
}