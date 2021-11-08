using System;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Nodes;
using test_app.Generated.Reactive;

namespace test_app.Components
{
    public class Menu : BaseComponent
    {
        public Menu(DependencyManager dependencyManager, JsManipulator jsManipulator, Store.Store store) : base(dependencyManager, jsManipulator)
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