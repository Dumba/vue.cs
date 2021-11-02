using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Elements
{
    public interface IElementBuilder
    {
        IElement Element { get; }

        Task InsertToDomAsync(JsManipulator jsManipulator, string parentId, BaseComponent parentComponent);
    }
}