using System;
using System.Collections.Generic;
using Vue.cs.Framework.Base;
using Vue.cs.Framework.Runtime.Nodes.Builders;
using Vue.cs.Framework.Runtime.Nodes.Interfaces;

namespace test.Components
{
    public class Menu : BaseComponent
    {
        public Menu(IServiceProvider serviceProvider, Store.Store store) : base(serviceProvider)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        public override void Setup(Builder builder, IEnumerable<IPageItem>? childNodes = null)
        {
            builder.AddChild("menu", menu => menu
                .AddClass("main")
                .AddChild("ul", ul => ul
                    .AddChild("li", li => li
                        .AddText("Home")
                        .AddEventListener("click", "MoveTo", "home"))
                    .AddChild("li", li => li
                        .AddText("About")
                        .AddEventListener("click", "MoveTo", "about")))
                .AddChild("span", ch => ch
                    .AddText("Hidden Menu")
                    .SetCondition(_store.Hidden)));
        }

        [Microsoft.JSInterop.JSInvokable]
        public void MoveTo(Event ev, string path)
        {
            System.Console.WriteLine($"Moved to {path}");
        }
    }
}