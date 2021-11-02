using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Elements;

namespace test_app.Components
{
    public class Menu : BaseComponent
    {
        public Menu(JsManipulator jsManipulator)
        {
            _jsManipulator = jsManipulator;
        }

        private JsManipulator _jsManipulator;

        protected override async Task<IElement> BuildBody(string parentId)
        {
            var builder = new ElementBuilder("menu")
                .AddClass("main")
                .AddChild("ul", null, ul => ul
                    .AddChild("li", null, li => li
                        .AddText("Home")
                        .AddEventListener("click", "MoveTo", "home")));

            await builder.InsertToDomAsync(_jsManipulator, parentId, this);
            
            return builder.Element;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void MoveTo(Event ev, string path)
        {
            System.Console.WriteLine($"Moved to {path}");
        }
    }
}