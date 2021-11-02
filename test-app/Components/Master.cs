using System.Threading.Tasks;
using test_app.Base;
using test_app.Generated;
using test_app.Generated.Elements;

namespace test_app.Components
{
    public class Master : BaseComponent
    {
        public Master(JsManipulator jsManipulator)
        {
            _jsManipulator = jsManipulator;
        }

        private JsManipulator _jsManipulator;

        protected override async Task<IElement> BuildBody(string parentId)
        {
            var builder = new ElementBuilder("div")
                .AddChild(new Menu(_jsManipulator))
                .AddText("hello")
                .AddChild("button", null, ch => ch
                    .AddEventListener("click", "Test", "Hello", "hal")
                    .AddText("Click"));
            
            await builder.InsertToDomAsync(_jsManipulator, parentId, this);

            return builder.Element;
        }

        [Microsoft.JSInterop.JSInvokable]
        public void Test(Event ev, string message, string another)
        {
            System.Console.WriteLine($"Clicked! :-) - {message} - {another}");
        }
    }
}