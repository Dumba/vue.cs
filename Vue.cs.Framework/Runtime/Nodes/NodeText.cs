using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeText : IPageNode
    {
        public NodeText(string? text, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            _text = text;
        }
        public NodeText(IReactiveProvider<string?> reactiveText, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            _reactiveText = reactiveText;
        }

        private string? _text;
        private IReactiveProvider<string?>? _reactiveText;

        public Guid Id { get; }
        public string? Text => _reactiveText is not null ? _reactiveText.Get(null) : _text;

        [JsonIgnore]
        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        [JsonIgnore]
        public bool IsVisible => true;
    }
}