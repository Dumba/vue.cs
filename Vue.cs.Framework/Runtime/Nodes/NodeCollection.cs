using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue.cs.Framework.Extensions;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes
{
    public class NodeCollection<TItem> : IPageItem, IReactiveCollectionConsumer<TItem>
    {
        public NodeCollection(IReactiveCollectionProvider<TItem> collectionProvider, Func<TItem, IPageItem> setupChild)
        {
            _setupChild = setupChild;
            _collectionProvider = collectionProvider;
            _mapping = new List<KeyValuePair<NullObject<TItem>, IPageItem>>();

            var startId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            _startNode = new($" collection {startId} ", startId);
            _endNode = new($" collection {startId} ", endId);

            foreach (var item in _collectionProvider.Value)
            {
                CreateAndRegister(item);
            }
        }

        private NodeComment _startNode;
        private NodeComment _endNode;

        private DependencyManager? _dependencyManager;
        private JsManipulator? _jsManipulator;
        private IReactiveCollectionProvider<TItem> _collectionProvider;
        private Func<TItem, IPageItem> _setupChild;
        private List<KeyValuePair<NullObject<TItem>, IPageItem>> _mapping;

        public IEnumerable<IPageNode> Nodes => _mapping
            .SelectMany(i => i.Value.Nodes)
            .Prepend(_startNode)
            .Append(_endNode);

        public IEnumerable<INodeBuilt> Build(DependencyManager dependencyManager, JsManipulator jsManipulator)
        {
            _dependencyManager = dependencyManager;
            _jsManipulator = jsManipulator;

            _dependencyManager.RegisterDependency(this, _collectionProvider);

            return _startNode.Build(_dependencyManager, _jsManipulator)
                .Concat(_mapping.SelectMany(i => i.Value.Build(dependencyManager, jsManipulator)))
                .Concat(_endNode.Build(_dependencyManager, _jsManipulator));
        }

        public void Demolish()
        {
            _startNode.Demolish();
            _endNode.Demolish();

            foreach (var item in _mapping)
            {
                item.Value.Demolish();
            }
        }

        public async ValueTask Added(TItem value)
        {
            if (_jsManipulator is null)
                return;

            var newNode = CreateAndRegister(value);

            await _jsManipulator.InsertNodeBefore(newNode, _endNode.Id);
        }

        public async ValueTask Removed(TItem value)
        {
            if (_jsManipulator is null)
                return;

            var pair = _mapping.First(p => p.Key.Equals(value));

            pair.Value.Demolish();

            _mapping.Remove(pair);

            foreach (var item in pair.Value.Nodes)
            {
                await _jsManipulator.RemoveNode(item.Id);
            }
        }

        private IPageItem CreateAndRegister(TItem item)
        {
            var newNode = _setupChild(item);

            _mapping.Add(new KeyValuePair<NullObject<TItem>, IPageItem>(item, newNode));
            return newNode;
        }
    }
}