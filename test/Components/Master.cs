using System;
using System.Collections.Generic;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Runtime.Nodes.Builders;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace test.Components
{
    public class Master : BaseComponent
    {
        public Master(IServiceProvider serviceProvider, Store.Store store) : base(serviceProvider)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        public override void Setup(Builder builder, IEnumerable<IPageItem>? childNodes = null)
        {
            builder
                .AddChild<Menu>(ch => ch
                    .SetCondition(_store.ShowText)
                    .AddClass("bleee"))
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
            _store.Message.Set(message!);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Add(Event ev)
        {
            var message = _store.Message.Value;
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
            _store.ShowText.Set(!_store.ShowText.Value);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void ToggleHide2(Event ev)
        {
            _store.Hidden.Set(!_store.Hidden.Value);
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Remove(Event ev, string item)
        {
            _store.List.Remove(item);
        }
    }
}