using System;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Nodes;
using test_app.Generated.Reactive;

namespace test_app.Components
{
    public class Master : BaseComponent
    {
        public Master(JsManipulator jsManipulator, DependencyManager dependencyManager)
        {
            _jsManipulator = jsManipulator;
            _dependencyManager = dependencyManager;
            Message = new ReactiveValue<string>(_dependencyManager, "Click?");
            Label = new ReactiveGetter<string, string>(_dependencyManager, Message, msg => $"{msg} :-)");
        }

        private readonly JsManipulator _jsManipulator;
        private readonly DependencyManager _dependencyManager;

        public ReactiveValue<string> Message;
        public ReactiveGetter<string, string> Label;

        protected override async Task<INode> BuildBody(Guid parentId)
        {
            var builder = new ElementBuilder("div")
                .AddChild(new Menu(_jsManipulator))
                .AddText("hello")
                .AddChild("input", ch => ch
                    .AddAttribute("value", Message.Get())
                    .AddEventListener("keyup", "Test", "ble", "ble"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "Test", "Hello", "hal")
                    .AddText(Label, _dependencyManager, _jsManipulator)
                    .AddText("Click? :-)"));
            
            await builder.InsertToDomAsync(_jsManipulator, parentId, this);

            return builder.Node;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Test(Event ev, string message, string another)
        {
            Message.Set(ev.Value);
        }
    }
}