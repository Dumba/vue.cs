using System.Collections.Generic;
using System.Linq;

namespace test_app.Generated
{
    public class AttributeCollection
    {
        public AttributeCollection(Element belongsToElement, JsManipulator jsHandler, Dictionary<string, string> attributes = null)
        {
            _belongsToElement = belongsToElement;
            _jsHandler = jsHandler;

            _innerDict = attributes ?? new Dictionary<string, string>();
        }

        private Element _belongsToElement;
        private JsManipulator _jsHandler;
        private Dictionary<string, string> _innerDict;

        public string Get(string name)
        {
            return _innerDict[name];
        }

        public void Set(string name, string value)
        {
            _innerDict[name] = value;
            
            _jsHandler.SetAttribute(_belongsToElement.Id, name, value);
        }

        public void Remove(string name)
        {
            _innerDict.Remove(name);
            
            _jsHandler.RemoveAttribute(_belongsToElement.Id, name);
        }
    }
}