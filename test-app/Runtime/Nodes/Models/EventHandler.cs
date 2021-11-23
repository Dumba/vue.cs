using test_app.Base;

namespace test_app.Runtime.Nodes.Models
{
    public class EventHandlerData
    {
        public string Event { get; set; }
        public BaseComponent Component { get; set; }
        public string ComponentMethodName { get; set; }
        public object[] Params { get; set; }
    }
}