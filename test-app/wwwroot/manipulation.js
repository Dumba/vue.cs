// private
function _getMapping() {
    if (!window.VueCs)
        window.VueCs = {};
    if (!window.VueCs.nodeMapping)
        window.VueCs.nodeMapping = {};

    return window.VueCs.nodeMapping;
}
function _getNode(id) {
    // guid
    if (new RegExp('[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}').test(id)) {
        return _getMapping()[id];
    }

    // any selector (master element)
    return document.querySelector(id);
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

function _createElement(tagName, id, attributes) {
    var element = document.createElement(tagName);
    _getMapping()[id] = element;

    if (typeof(attributes) == 'object') {
        Object.keys(attributes).forEach(key => {
            element.setAttribute(key, attributes[key]);
        });
    }
    
    return element;
}
function _createText(text, id) {
    var node = document.createTextNode(text);
    var mapping = _getMapping();
    mapping[id] = node;

    return node;
}
function _createComment(content, id) {
    var node = document.createComment(content);
    var mapping = _getMapping();
    mapping[id] = node;

    return node;
}
function _createNode(node) {
    if (node.text != null) {
        return _createText(node.text, node.id);
    }

    if (node.content != null) {
        return _createComment(node.content, node.id);
    }

    return _createElement(node.tagName, node.id, node.allAttributes);
}
function _insert(newNode, parentElement, nextNode = null) {
    const createdNode = _createNode(newNode);

    if (nextNode == null) {
        parentElement.appendChild(createdNode);
    } else {
        parentElement.insertBefore(createdNode, nextNode);
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
function InsertNode(parentElementId, newNode, nextNodeId = null) {
    try {
        const parentElement = _getNode(parentElementId);
        const nextNode = nextNodeId != null
            ? _getNode(nextNodeId)
            : null;

        _insert(newNode, parentElement, nextNode);
    }
    catch (err) {
        console.error(err);
    }
}
function InsertNodeBefore(newNode, nextNodeId) {
    try {
        const nextNode = _getNode(nextNodeId);
        _insert(newNode, nextNode.parentElement, nextNode);
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
function ReplaceNode(nodeId, newNode) {
    try {
        var oldNode = _getNode(nodeId);
        _insert(newNode, oldNode.parentElement, oldNode);
        oldNode.parentElement.removeChild(oldNode);
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
