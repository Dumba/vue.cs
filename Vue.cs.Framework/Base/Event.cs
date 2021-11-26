using System.Text.Json;

namespace Vue.cs.Framework.Base
{
    public class Event
    {
        public string Type { get; set; }
        public string TargetId { get; set; }
        public JsonElement Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int PageX { get; set; }
        public int PageY { get; set; }
    }
}