using System;
using System.Collections.Generic;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes.Models
{
    public record CommentBuilt(Guid Id, string? Content) : INodeBuilt;
    public record TextBuilt(Guid Id, string? Text) : INodeBuilt;
    public record ElementBuilt(Guid Id, string TagName, Dictionary<string, string?> Attributes, IEnumerable<EventHandlerData> EventHandlers, object[] Children) : INodeBuilt;
}