using System;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace Vue.cs.Framework.Runtime.Nodes.Builders
{
    public class TemplateBuilder : Builder
    {
        public TemplateBuilder(IServiceProvider serviceProvider, BaseComponent parentComponent, string? commentText = null)
            : base(serviceProvider, parentComponent, "")
        {
            var startId = Guid.NewGuid();
            var endId = Guid.NewGuid();

            _startNode = new NodeComment($" {commentText ?? "template"} {startId} ", startId);
            _endNode = new NodeComment($" {commentText ?? "template"} {startId} ", endId);
        }

        private NodeComment _startNode;
        private NodeComment _endNode;

        protected override IPageItemCollection CreatePageItem()
        {
            return new Template(_startNode, _endNode);
        }
    }
}