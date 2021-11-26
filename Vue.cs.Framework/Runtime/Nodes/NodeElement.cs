using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeElement : IPageNode, INodeParent, IPageItemCollection, IReactiveConsumer<bool>
    {
        public NodeElement(string tagName, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            TagName = tagName;
            Classes = new List<string>();
            Attributes = new HashSet<Attribute>();
            EventHandlers = new HashSet<EventHandlerData>();

            Children = new List<IPageItem>();
        }

        private DependencyManager? _dependencyManager;
        private JsManipulator? _jsManipulator;

        public Guid Id { get; }
        public string TagName { get; }
        public List<string> Classes { get; set; }
        public HashSet<Attribute> Attributes { get; set; }
        public HashSet<EventHandlerData> EventHandlers { get; set; }

        public List<IPageItem> Children { get; set; }
        public IReactiveProvider<bool>? Condition { get; set; }

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        public List<IPageItem> InnerNodes { get => Children; set => Children = value; }
        public bool IsVisible => Condition?.Value ?? true;

        public object Build(DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            foreach (var attribute in Attributes)
            {
                attribute.Build(dependencyManager, jsManipulator, Id);
            }

            var allAttributes = Attributes.ToDictionary(a => a.Name, a => a.Value);
            if (Classes.Any())
                allAttributes.Add("class", string.Join(" ", Classes));
            // if (Styles.Any())
            //     result.Add("style", string.Join("", Styles.Select(pair => $"{pair.Key}:{pair.Value};")));

            return new
            {
                Id,
                TagName,
                Attributes = allAttributes,
                EventHandlers,
            };
        }

        public void Demolish()
        {
        }

        public async ValueTask Changed(bool oldValue, bool newValue)
        {
            if (_jsManipulator is null)
                return;

            // show
            if (newValue)
            {
                await _jsManipulator.ReplaceNode(this);

                foreach (var child in Children)
                {
                    await child.Render(_jsManipulator, Id);
                }
            }
            // hide
            else
            {
                await _jsManipulator.ReplaceNode(new NodeComment(id: Id));
            }
        }
    }
}