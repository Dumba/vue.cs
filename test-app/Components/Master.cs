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
                    .AddEventListener("keyup", "SetMessage"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "Add")
                    .AddText(_store.Label)
                    .AddText("Click? :-)"))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "ToggleHide")
                    .AddText(_store.ShowHideLabel))
                .AddChild("button", ch => ch
                    .AddEventListener("click", "ToggleHide2")
                    .AddText("ShowHideLabel - 2"))
                .AddChildren(_store.List, "button", (ch, i) => ch
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
        public void SetMessage(Event ev)
        {
            var message = ev.Value.GetString();
            _store.Message.Set(message);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Add(Event ev)
        {
            var message = _store.Message.Get(null);
            if (string.IsNullOrEmpty(message))
            {
                System.Console.WriteLine("Empty value");
                return;
            }

            _store.List.Add(message);
            _store.Message.Set("");
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