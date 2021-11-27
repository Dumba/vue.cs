using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Nodes.Models;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeText : IPageNode, IReactiveConsumer<string?>
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

        private DependencyManager? _dependencyManager;
        private JsManipulator? _jsManipulator;
        private string? _text;
        private IReactiveProvider<string?>? _reactiveText;

        public Guid Id { get; }
        public string? Text => _reactiveText is not null ? _reactiveText.Value : _text;

        public IEnumerable<IPageNode> Nodes { get { yield return this; } }
        public bool IsVisible => true;

        public IEnumerable<INodeBuilt> Build(DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            if (_reactiveText is not null)
                _dependencyManager.RegisterDependency(this, _reactiveText);

            yield return new TextBuilt(Id, Text);
        }

        public void Demolish()
        {
            if (_reactiveText is not null)
                _dependencyManager?.UnregisterDependency(this, _reactiveText);

            _dependencyManager = null;
            _jsManipulator = null;
        }

        public async ValueTask Changed(string? oldValue, string? newValue)
        {
            if (_jsManipulator is null)
                return;

            await _jsManipulator.UpdateText(Id, newValue);
        }
    }
}