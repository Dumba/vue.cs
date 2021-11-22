using System;
using System.Threading.Tasks;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive
{
    public class ReactiveText : IReactiveConsumer<string>
    {
        public ReactiveText(JsManipulator jsManipulator)
        {
            _jsManipulator = jsManipulator;
        }

        private readonly JsManipulator _jsManipulator;

        public Guid PageItemId { get; }

        public ValueTask Changed(string oldValue, string newValue)
        {
            return _jsManipulator.UpdateText(PageItemId, newValue);
        }
    }
}