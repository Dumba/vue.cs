using System;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes.Builders
{
    public class TemplateBuilder : Builder
    {
        public TemplateBuilder(IServiceProvider serviceProvider, BaseComponent parentComponent) : base(serviceProvider, parentComponent, "")
        {
            var startId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            StartNode = new NodeComment($" start template {startId} / {endId} ", startId);
            EndNode = new NodeComment($" end template {startId} / {endId} ", endId);
        }

        public NodeComment StartNode { get; set; }
        public NodeComment EndNode { get; set; }

        protected override IPageItemCollection CreatePageItem()
        {
            return new Template(StartNode, EndNode);
        }
    }
}