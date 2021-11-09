using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Nodes
{
    public class NodeElement : INode
    {
        public NodeElement(string tagName)
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


        public async ValueTask RenderAsync(JsManipulator jsManipulator, BaseComponent parentComponent, Guid parentElementId, Guid? insertBeforeNodeId = null)
        {
            // tag with attributes
            await jsManipulator.InsertNode(parentElementId, this, insertBeforeNodeId);

            // events
            foreach (var eventHandler in EventHandlers)
            {
                await jsManipulator.AddEventListener(Id, parentComponent, eventHandler.@event, eventHandler.componentMethod, eventHandler.@params);
            }
        }
    }
}