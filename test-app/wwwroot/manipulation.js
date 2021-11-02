// private
function CreateNode(tagName, id, attributes) {
    var element = document.createElement(tagName);
    element.id = id;

    if (typeof(attributes) == 'object') {
        Object.keys(attributes).forEach(key => {
            element.setAttribute(key, attributes[key]);
        });
    }
    
    return element;
}
function SerializeEvent(event)
{
    return {
        type: event.type,
        targetId: event.target.id,
        x: event.x,
        y: event.y,
        pageX: event.pageX,
        pageY: event.pageY,
    }
}

// attributes
function SetAttribute(elementId, attributeName, attributeValue) {
    try {
        var element = document.getElementById(elementId);
        element.setAttribute(attributeName, attributeValue);
    }
    catch (err) {
        console.error(err);
    }
}
function RemoveAttribute(elementId, attributeName) {
    try {
        var element = document.getElementById(elementId);
        element.removeAttribute(attributeName);
    }
    catch (err) {
        console.error(err);
    }
}

// nodes
function InsertContent(elementId, newElement, nodeIndex = null) {
    try {
        const element = document.getElementById(elementId);

        if (nodeIndex == null) {
            nodeIndex = element.childNodes.length;
        }

        const node = newElement.text != null
            ? document.createTextNode(newElement.text)
            : CreateNode(newElement.tagName, newElement.id, newElement.attributes);

        if (nodeIndex >= element.childNodes.length) {
            element.appendChild(node);
        } else {
            const afterElement = element.childNodes[nodeIndex];
            element.insertBefore(node, afterElement);
        }
    }
    catch (err) {
        console.error(err);
    }
}
function RemoveContent(elementId, nodeIndex = null) {
    try {
        var element = document.getElementById(elementId);

        if (nodeIndex == null) {
            nodeIndex = element.childNodes.length - 1;
        }

        element.removeChild(element.childNodes[nodeIndex]);
    }
    catch (err) {
        console.error(err);
    }
}
function UpdateContent(elementId, newElement, nodeIndex = null) {
    RemoveContent(elementId, nodeIndex);
    InsertContent(elementId, newElement, nodeIndex);
}

// events
function AddListener(elementId, eventName, caller, methodName, params)
{
    var element = document.getElementById(elementId);
    element[`on${eventName}`] = (ev) => caller.invokeMethod(methodName, SerializeEvent(ev), ...params);
}
