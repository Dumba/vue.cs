using System.Threading.Tasks;
using test_app.Base;

namespace test_app.Generated.Elements
{
    public class ComponentToElementBuilder : IElementBuilder
    {
        public ComponentToElementBuilder(BaseComponent component)
        {
            _component = component;
        }

        private BaseComponent _component;

        public IElement Element => throw new System.NotImplementedException();

        public Task InsertToDomAsync(JsManipulator jsManipulator, string parentId, BaseComponent parentComponent)
        {
            return _component.RenderAsync(parentId);
        }
    }
}