using Microsoft.JSInterop;
using Vue.cs.Framework.Base;

namespace Vue.cs.Framework.Runtime.Nodes.Models
{
    public class EventHandlerData
    {
        public string Event { get; set; }
        public DotNetObjectReference<BaseComponent> ComponentInterop { get; set; }
        public string ComponentMethodName { get; set; }
        public object[] Params { get; set; }
    }
}