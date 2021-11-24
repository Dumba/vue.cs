using Microsoft.JSInterop;
using test_app.Base;

namespace test_app.Runtime.Nodes.Models
{
    public class EventHandlerData
    {
        public string Event { get; set; }
        public DotNetObjectReference<BaseComponent> ComponentInterop { get; set; }
        public string ComponentMethodName { get; set; }
        public object[] Params { get; set; }
    }
}