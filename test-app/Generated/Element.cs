using System;
using System.Collections.Generic;

namespace test_app.Generated
{
    public class Element : IElement
    {
        public Element(JsManipulator jsHandler, string tagName, string id = null, Dictionary<string, string> attributes = null, IEnumerable<IElement> children = null)
        {
            TagName = tagName;
            Id = id ?? Guid.NewGuid().ToString();

            Attributes = new AttributeCollection(this, jsHandler, attributes);
            ChildElements = new ChildCollection(this, jsHandler, children);
        }

        public string TagName { get; }
        public string Id { get; }
        public AttributeCollection Attributes { get; set; }
        public ChildCollection ChildElements { get; set; }
    }
}