using System;
using test_app.Base;
using test_app.Runtime.Nodes.Builders;

namespace test_app.Components
{
    public class Master : BaseComponent
    {
        public Master(IServiceProvider serviceProvider, Store.Store store) : base(serviceProvider)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        public override void Setup(TemplateBuilder builder)
        {
            builder
                .AddChild<Menu>(ch => ch
                    .SetCondition(_store.ShowText))
                .AddText("hello")
                .AddChild("input", ch => ch
                    .AddClass("ccc")
                    .AddAttribute("value", _store.Message)
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
                .AddChildren(_store.List, "span", (ch, i) => ch
                    .AddText(i)
                    .AddEventListener("click", "Remove", i))
                .AddChild("div", ch => ch
                    .SetCondition(_store.ShowText)
                    .AddText("Vidíš mě?"))
                .AddChild("div", ch => ch
                    .SetCondition(_store.Hidden)
                    .AddText("next hidden"))
                .AddChild("div", ch => ch
                    .AddText("Poslední"))
                ;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Test(Event ev, string message, string another)
        {
            _store.Message.Set(string.IsNullOrEmpty(ev.Value) ? message : ev.Value);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide(Event ev)
        {
            _store.ShowText.Set(!_store.ShowText.Get(null));
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide2(Event ev)
        {
            _store.Hidden.Set(!_store.Hidden.Get(null));
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Remove(Event ev, string item)
        {
            _store.List.Remove(item);
        }
    }
}