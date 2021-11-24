using System;
using test_app.Base;
using test_app.Runtime.Nodes.Builders;

namespace test_app.Components
{
    public class Menu : BaseComponent
    {
        public Menu(IServiceProvider serviceProvider, Store.Store store) : base(serviceProvider)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        public override void Setup(TemplateBuilder builder)
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