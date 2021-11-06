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
        public Master(DependencyManager dependencyManager, JsManipulator jsManipulator) : base(dependencyManager, jsManipulator)
        {
            Message = new ReactiveValue<string>(_dependencyManager, "Click?");
            Label = new ReactiveGetter<string, string>(_dependencyManager, Message, msg => $"{msg} :-)");
            ShowText = new ReactiveValue<bool>(_dependencyManager);
            ShowHideLabel = new ReactiveGetter<string, bool>(_dependencyManager, ShowText, visible => visible ? "Hide" : "Show");
        }

        public ReactiveValue<string> Message;
        public ReactiveGetter<string, string> Label;
        public ReactiveValue<bool> ShowText;
        public ReactiveGetter<string, bool> ShowHideLabel;

        protected override async Task<INode> BuildBody(Guid parentId, Guid? insertBeforeNodeId = null)
        {
            var builder = CreateRoot(parentId, "div")
#warning Tohle se mi moc nelíbí... Možná k tomu vytvořit nějakého ComponentBuildera?
                .AddChild(new Menu(_dependencyManager, _jsManipulator))
                .AddText("hello")
                .AddChild("input", ch => ch
                    .AddAttribute("value", Message.Get())
                    .AddEventListener("keyup", "Test", "ble", "ble"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "Test", "Hello", "hal")
                    .AddText(Label)
                    .AddText("Click? :-)"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "ToggleHide")
                    .AddText(ShowHideLabel))
                .AddChild("div", ch => ch
                    .SetCondition(ShowText)
                    .AddText("Vidíš mě?"))
                .AddChild("div", ch => ch
                    .AddText("Poslední"));

            await builder.InsertToDomAsync(insertBeforeNodeId);

            return builder.Node;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Test(Event ev, string message, string another)
        {
            Message.Set(ev.Value);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide(Event ev)
        {
            ShowText.Set(!ShowText.Get());
        }
    }
}