using System;
using System.Threading.Tasks;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive.PageItems
{
    public class ReactiveText : IReactiveConsumer<string>
    {
        public ReactiveText(JsManipulator jsManipulator, Guid pageItemId)
        {
            _jsManipulator = jsManipulator;

            PageItemId = pageItemId;
        }

        private readonly JsManipulator _jsManipulator;

        public Guid PageItemId { get; }

        public ValueTask Changed(string oldValue, string newValue)
        {
            return _jsManipulator.UpdateText(PageItemId, newValue);
        }
        
        public class Builder
        {
            public Builder(JsManipulator jsManipulator)
            {
                _jsManipulator = jsManipulator;
            }

            private readonly JsManipulator _jsManipulator;

            public ReactiveText Build(Guid pageItemId, IReactiveProvider<string> valueProvider, out string initValue)
            {
                var reactiveText =  new ReactiveText(_jsManipulator, pageItemId);
                initValue = valueProvider.Get(reactiveText);
                return reactiveText;
            }
        }
    }
}