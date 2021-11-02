using System.Collections.Generic;
using System.Linq;

namespace test_app.Generated
{
    public class ChildCollection
    {
        public ChildCollection(Element belongsToElement, JsManipulator jsHandler, IEnumerable<IElement> children = null)
        {
            _belongsToElement = belongsToElement;
            _jsHandler = jsHandler;

            _innerList = children?.ToList() ?? new System.Collections.Generic.List<IElement>();
        }

        private Element _belongsToElement;
        private JsManipulator _jsHandler;
        private System.Collections.Generic.List<IElement> _innerList;

        public void Add(IElement value)
        {
            _innerList.Add(value);

            _jsHandler.InsertContent(_belongsToElement.Id, value);
        }

        public IElement Get(int index)
        {
            return _innerList[index];
        }

        public void RemoveAt(int index = 0)
        {
            _innerList.RemoveAt(index);

            _jsHandler.RemoveContent(_belongsToElement.Id, index);
        }
        
        public int Count => _innerList.Count;        
    }
}