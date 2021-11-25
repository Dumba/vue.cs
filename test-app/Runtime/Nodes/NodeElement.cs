using System;
using System.Collections.Generic;
using System.Linq;
using test_app.Runtime.Nodes.Interfaces;
using test_app.Runtime.Nodes.Models;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Nodes
{
    public class NodeElement : IPageNode, INodeParent, IPageItemWithAttrs
    {
        public NodeElement(string tagName)
        {
            Id = Guid.NewGuid();
            TagName = tagName;
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            Styles = new Dictionary<string, string>();
            EventHandlers = new List<EventHandlerData>();

            Children = new List<IPageItem>();
        }

        public Guid Id { get; }
        public string TagName { get; }
        public List<string> Classes { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Styles { get; }
        public List<EventHandlerData> EventHandlers { get; }

        public Dictionary<string, string> AllAttributes
        {
            get
            {
                var result = Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);
                if (Classes.Any())
                    result.Add("class", string.Join(" ", Classes));
                if (Styles.Any())
                    result.Add("style", string.Join("", Styles.Select(pair => $"{pair.Key}:{pair.Value};")));

                return result;
            }
        }

        public List<IPageItem> Children { get; }
        public IReactiveProvider<bool> Condition { get; set; }

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        public bool IsVisible => Condition?.Get(null) != false;
    }
}