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

function _createElement(node) {
    var element = document.createElement(node.tagName);
    _getMapping()[node.id] = element;

    if (typeof(node.attributes) == 'object') {
        Object.keys(node.attributes).forEach(key => {
            element.setAttribute(key, node.attributes[key]);
        });
    }

    if (node.eventHandlers?.length) {
        node.eventHandlers.forEach(eventHandler => {
            element[`on${eventHandler.event}`] = (ev) => eventHandler.componentInterop.invokeMethod(eventHandler.componentMethodName, _serializeEvent(ev), ...eventHandler.params);
        })
    }

    if (node.children?.length) {
        node.children.forEach(child => {
            _insert(child, element);
        });
    }
    
    return element;
}
function _createText(node) {
    var createdNode = document.createTextNode(node.text);
    var mapping = _getMapping();
    mapping[node.id] = createdNode;

    return createdNode;
}
function _createComment(node) {
    var createdNode = document.createComment(node.content);
    var mapping = _getMapping();
    mapping[node.id] = createdNode;

    return createdNode;
}
function _createNode(node) {
    if (node.text != null) {
        return _createText(node);
    }

    if (node.content != null) {
        return _createComment(node);
    }

    return _createElement(node);
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
        node.remove();
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
