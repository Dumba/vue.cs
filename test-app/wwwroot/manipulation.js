// private
function _getMapping() {
    if (!window.VueCs)
        window.VueCs = {};
    if (!window.VueCs.nodeMapping)
        window.VueCs.nodeMapping = {};

    return window.VueCs.nodeMapping;
}
function _createElement(tagName, id, attributes) {
    var element = document.createElement(tagName);
    var mapping = _getMapping();
    mapping[id] = element;

    if (typeof(attributes) == 'object') {
        Object.keys(attributes).forEach(key => {
            element.setAttribute(key, attributes[key]);
        });
    }
    
    return element;
}
function _createText(text, id) {
    var element = document.createTextNode(text);
    var mapping = _getMapping();
    mapping[id] = element;

    return element;
}
function _getNode(id) {
    var mapping = _getMapping();
    // mapping or master component
    var node = mapping[id] ?? document.querySelector(id);

    return node;
}
function _serializeEvent(event)
{
    return {
        type: event.type,
        targetId: event.target.id,
        value: event.target.value ?? "",
        x: event.x,
        y: event.y,
        pageX: event.pageX,
        pageY: event.pageY,
    }
}

// attributes
function SetAttribute(elementId, attributeName, attributeValue) {
    try {
        var element = _getNode(elementId);

        // value sometime doesn't work with setAttribute
        if (attributeName == "value") {
            element.value = attributeValue;
        } else {
            element.setAttribute(attributeName, attributeValue);
        }
    }
    catch (err) {
        console.error(err);
    }
}
function RemoveAttribute(elementId, attributeName) {
    try {
        var element = _getNode(elementId);
        element.removeAttribute(attributeName);
    }
    catch (err) {
        console.error(err);
    }
}

// nodes
function InsertNode(parentElementId, newNode, insertBeforeNodeId = null) {
    try {
        const parentElement = _getNode(parentElementId);

        const createdNode = newNode.text != null
            ? _createText(newNode.text, newNode.id)
            : _createElement(newNode.tagName, newNode.id, newNode.attributes);

        if (insertBeforeNodeId == null) {
            parentElement.appendChild(createdNode);
        } else {
            var nextNode = _getNode(insertBeforeNodeId);
            parentElement.insertBefore(createdNode, nextNode);
        }
    }
    catch (err) {
        console.error(err);
    }
}
function RemoveNode(nodeId) {
    try {
        var node = _getNode(nodeId);

        node.parentElement.removeChild(node);
    }
    catch (err) {
        console.error(err);
    }
}
function UpdateText(nodeId, newText) {
    try {
        var node = _getNode(nodeId);
        node.data = newText;
    }
    catch (err) {
        console.error(err);
    }
}

// events
function AddListener(elementId, eventName, eventTarget, methodName, params)
{
    var element = _getNode(elementId);
    element[`on${eventName}`] = (ev) => eventTarget.invokeMethod(methodName, _serializeEvent(ev), ...params);
}
