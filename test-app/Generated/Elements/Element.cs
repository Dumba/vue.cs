using System;
using System.Collections.Generic;

namespace test_app.Generated.Elements
{
    public class Element : IElement
    {
        public Element(string tagName, string id = null)
        {
            TagName = tagName;
            Id = id ?? Guid.NewGuid().ToString();

            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            EventHandlers = new List<(string @event, string componentMethod, object[] @params)>();
        }

        public string TagName { get; }
        public string Id { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public List<(string @event, string componentMethod, object[] @params)> EventHandlers { get; }
    }
}