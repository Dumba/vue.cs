using System;
using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Nodes;
using test_app.Generated.Reactive;

namespace test_app.Components
{
    public class Menu : BaseComponent
    {
        public Menu(DependencyManager dependencyManager, JsManipulator jsManipulator) : base(dependencyManager, jsManipulator)
        {
        }

        protected override async Task<INode> BuildBody(Guid parentId, Guid? insertBeforeNodeId = null)
        {
            var builder = CreateRoot(parentId, "menu")
                .AddClass("main")
                .AddChild("ul", ul => ul
                    .AddChild("li", li => li
                        .AddText("Home")
                        .AddEventListener("click", "MoveTo", "home")));

            await builder.InsertToDomAsync(insertBeforeNodeId);
            
            return builder.Node;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void MoveTo(Event ev, string path)
        {
            System.Console.WriteLine($"Moved to {path}");
        }
    }
}