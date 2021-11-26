using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;
using Vue.cs.Framework.Runtime.Reactive.PageItems;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeElement : IPageNode, INodeParent, IPageItemBuild
    {
        public NodeElement(string tagName, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            TagName = tagName;
            Classes = new List<string>();
            Attributes = new Dictionary<string, string>();
            EventHandlers = new HashSet<EventHandlerData>();

            Children = new List<IPageItem>();
        }

        public Guid Id { get; }
        public string TagName { get; }
        [JsonIgnore]
        public List<string> Classes { get; set; }
        [JsonIgnore]
        public Dictionary<string, string> Attributes { get; set; }
        public HashSet<EventHandlerData> EventHandlers { get; set; }

        public Dictionary<string, string> AllAttributes
        {
            get
            {
                var result = Attributes.ToDictionary(pair => pair.Key, pair => pair.Value);
                if (Classes.Any())
                    result.Add("class", string.Join(" ", Classes));
                // if (Styles.Any())
                //     result.Add("style", string.Join("", Styles.Select(pair => $"{pair.Key}:{pair.Value};")));

                return result;
            }
        }

        [JsonIgnore]
        public List<IPageItem> Children { get; set; }
        [JsonIgnore]
        public IReactiveProvider<bool> Condition { get; set; }

        [JsonIgnore]
        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        [JsonIgnore]
        public List<IPageItem> InnerNodes { get => Children; set => Children = value; }
        [JsonIgnore]
        public bool IsVisible => Condition?.Get(null) ?? true;

        public void AddClass(string className)
        {
            Classes.Add(className);
        }
        public void AddAttribute(KeyValuePair<string, string> attribute)
        {
            Attributes[attribute.Key] = attribute.Value;
        }
        public void AddEventHandler(EventHandlerData eventHandler)
        {
            EventHandlers.Add(eventHandler);
        }
        public void AddReactiveAttribute(IServiceProvider serviceProvider, string attributeName, IReactiveProvider<string> valueProvider)
        {
            var reactiveAttribute = serviceProvider.GetService<ReactiveAttribute.Builder>()
                .Build(Id, attributeName, valueProvider, out var text);

            Attributes.Add(attributeName, text);
        }
    }
}