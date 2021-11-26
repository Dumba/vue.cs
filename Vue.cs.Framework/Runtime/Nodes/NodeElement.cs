using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Vue.cs.Framework.Extensions;
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
            Attributes = new HashSet<Attribute>();
            EventHandlers = new HashSet<EventHandlerData>();
            Conditions = new HashSet<IReactiveProvider<bool>>();

            Children = new List<IPageItem>();
        }

        public Guid Id { get; }
        public string TagName { get; }
        public List<string> Classes { get; set; }
        public HashSet<Attribute> Attributes { get; set; }
        public HashSet<EventHandlerData> EventHandlers { get; set; }

        public Dictionary<string, string?> AllAttributes
        {
            get
            {
                var result = Attributes.ToDictionary(a => a.Name, a => a.Value);
                if (Classes.Any())
                    result.Add("class", string.Join(" ", Classes));
                // if (Styles.Any())
                //     result.Add("style", string.Join("", Styles.Select(pair => $"{pair.Key}:{pair.Value};")));

                return result;
            }
        }

        public List<IPageItem> Children { get; set; }
        public HashSet<IReactiveProvider<bool>> Conditions { get; set; }
        private IReactiveProvider<bool>? _finalCondition;

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        public List<IPageItem> InnerNodes { get => Children; set => Children = value; }
        public bool IsVisible => _finalCondition?.Value ?? true;

        public void AddClass(string className)
        {
            Classes.Add(className);
        }
        public void AddAttribute(Attribute attribute)
        {
            Attributes.Add(attribute);
        }
        public void AddEventHandler(EventHandlerData eventHandler)
        {
            EventHandlers.Add(eventHandler);
        }
        public void AddCondition(IReactiveProvider<bool> condition)
        {
            Conditions.Add(condition);
        }

        public void BuildCondition(IServiceProvider serviceProvider)
        {
            if (!Conditions.Any())
                return;

            #warning TODO: mix multiple conditions
            if (Conditions.Count() > 1)
                Console.WriteLine("hoops");

            _finalCondition = Conditions.First();

            serviceProvider.Get<ReactivePageItem.Builder>()
                .Build(this, _finalCondition);
        }

        public object Build()
        {
            return new
            {
                Id,
                TagName,
                AllAttributes,
                EventHandlers,
            };
        }
    }
}