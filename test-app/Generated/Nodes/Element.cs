using System;
using System.Collections.Generic;
using System.Linq;

namespace test_app.Generated.Nodes
{
    public class Element : INode
    {
        public Element(string tagName)
        {
            TagName = tagName;
            Id = Guid.NewGuid();

            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            Styles = new Dictionary<string, string>();
            EventHandlers = new List<(string @event, string componentMethod, object[] @params)>();
        }

        public string TagName { get; }
        public Guid Id { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Styles { get; }
        public List<(string @event, string componentMethod, object[] @params)> EventHandlers { get; }
        
        public object Serialize()
        {
            var allAttributes = Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);
            if (Classes.Any())
                allAttributes.Add("class", string.Join(" ", Classes));
            if (Styles.Any())
                allAttributes.Add("style", string.Join("", Styles.Select(pair => $"{pair.Key}:{pair.Value};")));

            return new {
                Id,
                TagName,
                Attributes = allAttributes,
                EventHandlers,
            };
        }
    }
}