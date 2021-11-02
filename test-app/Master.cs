using System;
using System.Text.Json;
using test_app.Components;
using test_app.Generated;

namespace test_app
{
    public class Master : BaseComponent
    {

        public Master(JsManipulator jsManipulator)
        {
            _jsManipulator = jsManipulator;

            Parent = new Element(_jsManipulator, "body", "app");
        }

        private JsManipulator _jsManipulator;

        public override IElement BuildBody()
        {
            var body = new Element(_jsManipulator, "div");

            return body;
        }
    }
}