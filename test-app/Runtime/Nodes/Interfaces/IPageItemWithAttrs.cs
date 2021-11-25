using System.Collections.Generic;
using test_app.Runtime.Nodes.Models;

namespace test_app.Runtime.Nodes.Interfaces
{
    public interface IPageItemWithAttrs
    {
        List<string> Classes { get; }
        Dictionary<string, string> Attributes { get; }
        Dictionary<string, string> Styles { get; }
        List<EventHandlerData> EventHandlers { get; }
    }
}