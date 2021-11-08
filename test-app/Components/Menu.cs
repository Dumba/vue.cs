using System;
using test_app.Base;
using test_app.Generated.Nodes;

namespace test_app.Components
{
    public class Menu : BaseComponent
    {
        public Menu(IServiceProvider serviceProvider, Store.Store store) : base(serviceProvider)
        {
            _store = store;
        }

        private readonly Store.Store _store;

        protected override INodeBuilder _buildNodes(Guid parentElementId)
        {
            var builder = CreateRoot(parentElementId, "menu")
                .AddClass("main")
                .AddChild("ul", ul => ul
                    .AddChild("li", li => li
                        .AddText("Home")
                        .AddEventListener("click", "MoveTo", "home")))
                .AddChild("span", ch => ch
                    .AddText("Hidden Menu")
                    .SetCondition(_store.Hidden));

            return builder;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void MoveTo(Event ev, string path)
        {
            System.Console.WriteLine($"Moved to {path}");
        }
    }
}