using System;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Nodes;
using test_app.Generated.Reactive;

namespace test_app.Components
{
    public class Master : BaseComponent
    {
        public Master(DependencyManager dependencyManager, JsManipulator jsManipulator, Store.Store store) : base(dependencyManager, jsManipulator)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        protected override ElementBuilder _buildNodes(Guid parentElementId)
        {
            var builder = CreateRoot(parentElementId, "div")
#warning Tohle se mi moc nelíbí... Možná k tomu vytvořit nějakého ComponentBuildera?
                .AddChild(new Menu(_dependencyManager, _jsManipulator, _store), ch => ch
                    .SetCondition(_store.ShowText))
                .AddText("hello")
                .AddChild("input", ch => ch
                    .AddClass("ccc")
                    .AddAttribute("value", _store.Message.Get())
                    .AddEventListener("keyup", "Test", "ble", "ble"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "Test", "Hello", "hal")
                    .AddText(_store.Label)
                    .AddText("Click? :-)"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "ToggleHide")
                    .AddText(_store.ShowHideLabel))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "ToggleHide2")
                    .AddText("ShowHideLabel - 2"))
                // .AddChildren(_store.List, "span", (ch, i) => ch
                //     .AddText(i)
                //     .AddEventListener("click", "Remove", i))
                .AddChild("div", ch => ch
                    .SetCondition(_store.ShowText)
                    .AddText("Vidíš mě?"))
                .AddChild("div", ch => ch
                    .SetCondition(_store.Hidden)
                    .AddText("next hidden"))
                .AddChild("div", ch => ch
                    .AddText("Poslední"));

            return builder;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Test(Event ev, string message, string another)
        {
            _store.Message.Set(ev.Value);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide(Event ev)
        {
            _store.ShowText.Set(!_store.ShowText.Get());
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide2(Event ev)
        {
            _store.Hidden.Set(!_store.Hidden.Get());
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Remove(Event ev, string item)
        {
            _store.List.Remove(item);
        }
    }
}