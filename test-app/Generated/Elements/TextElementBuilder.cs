using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Elements
{
    public class TextElementBuilder : IElementBuilder
    {
        public TextElementBuilder(string text)
        {
            Element = new TextElement(text);
        }

        public IElement Element { get; }

        public Task InsertToDomAsync(JsManipulator jsManipulator, string parentId, BaseComponent parentComponent)
        {
            jsManipulator.InsertContent(parentId, Element);

            return Task.CompletedTask;
        }
    }
}