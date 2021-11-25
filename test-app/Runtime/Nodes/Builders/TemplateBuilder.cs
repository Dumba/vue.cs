using System;
using test_app.Base;
using test_app.Runtime.Nodes.Interfaces;

namespace test_app.Runtime.Nodes.Builders
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

        protected override IPageItemBuild CreatePageItem()
        {
            return new Template(StartNode, EndNode);
        }
    }
}