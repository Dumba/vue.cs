using System;
using System.Threading.Tasks;
using Vue.cs.Framework.Runtime.Reactive.Interfaces;

namespace Vue.cs.Framework.Runtime.Reactive.PageItems
{
    public class ReactiveAttribute : IReactiveConsumer<string>
    {
        public ReactiveAttribute(JsManipulator jsManipulator, Guid pageItemId, string attributeName)
        {
            _jsManipulator = jsManipulator;

            PageItemId = pageItemId;
            AttributeName = attributeName;
        }

        private readonly JsManipulator _jsManipulator;

        public Guid PageItemId { get; }
        public string AttributeName { get; }

        public ValueTask Changed(string? oldValue, string? newValue)
        {
            return _jsManipulator.SetAttribute(PageItemId, AttributeName, newValue);
        }

        public class Builder
        {
            public Builder(JsManipulator jsManipulator)
            {
                _jsManipulator = jsManipulator;
            }

            private readonly JsManipulator _jsManipulator;

            public ReactiveAttribute Build(Guid pageItemId, string attributeName, IReactiveProvider<string> valueProvider, out string? initValue)
            {
                var reactiveAttribute = new ReactiveAttribute(_jsManipulator, pageItemId, attributeName);
                initValue = valueProvider.Get(reactiveAttribute);
                return reactiveAttribute;
            }
        }
    }
}