using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vue.cs.Generator.Expansions;
using Vue.cs.Generator.DomElements;

namespace Vue.cs.Generator.Workers
{
    public class Parser
    {
        public Parser(string html)
        {
            _html = html;
            _positionIndex = 0;
            _notClosedTags = new();
            _rootNodes = new();
        }

        private string _html;
        private int _positionIndex;
        private List<Element> _notClosedTags;
        private List<INode> _rootNodes;
        private Element? _active => _notClosedTags.LastOrDefault();

        public IEnumerable<INode> Parse()
        {
            while (TryGoNext(out var stringResult, out var type))
            {
                switch (type)
                {
                    case ENextType.Content:
                        var content = new Text(stringResult);
                        if (_active is not null)
                            _active.Children.Add(content);
                        else
                            _rootNodes.Add(content);

                        break;

                    case ENextType.TagStart:
                        var newTag = CreateTag(stringResult, _active);

                        _active?.Children.Add(newTag);
                        _notClosedTags.Add(newTag);
                        break;

                    case ENextType.TagStartEnd:
                        var newTag2 = CreateTag(stringResult, _active);

                        // closed main tag
                        if (_active is null)
                            _rootNodes.Add(newTag2);
                        else
                            _active.Children.Add(newTag2);
                        break;

                    case ENextType.TagEnd:
                        if (_active is null || _active.TagName != stringResult)
                            throw new System.Exception($"Tag {_active} was not closed!");

                        var removed = _notClosedTags.Last();
                        _notClosedTags.RemoveAt(_notClosedTags.Count - 1);

                        // all tags closed
                        if (!_notClosedTags.Any())
                            _rootNodes.Add(removed);

                        break;

                    case ENextType.Script:
                        var script = new Script(stringResult);
                        _rootNodes.Add(script);

                        break;
                    case ENextType.DocType:
                    case ENextType.Comment:
                        // ignore
                        break;

                    default:
                        throw new System.InvalidOperationException("Unknown type");
                }
            }

            if (_notClosedTags.Any())
                throw new System.Exception($"Following tags were not closed: {string.Join(", ", _notClosedTags)}");

            return _rootNodes;
        }

        private bool TryGoNext(out string result, out ENextType type)
        {
            var nextTagStartIndex = _html.IndexOfOrDefault("<", _positionIndex);
            var contentBeforeTag = _html.Cut(_positionIndex, nextTagStartIndex).Trim();

            // content
            if (nextTagStartIndex == null || contentBeforeTag.Length > 0)
            {
                result = contentBeforeTag;
                type = ENextType.Content;
                _positionIndex = nextTagStartIndex ?? _html.Length - 1;
                return contentBeforeTag.Length > 0;
            }

            // tag end
            if (_html.Length > _positionIndex && _html[nextTagStartIndex.Value + 1] == '/')
            {
                var nextTagEndIndex = _html.IndexOf('>', nextTagStartIndex.Value);
                var tag = _html.Cut(nextTagStartIndex + 2, nextTagEndIndex);

                result = tag;
                type = ENextType.TagEnd;
                _positionIndex = nextTagEndIndex + 1;
                return true;
            }

            // doctype or comment
            if (_html.Length > _positionIndex && _html[nextTagStartIndex.Value + 1] == '!')
            {
                if (_html.Cut(nextTagStartIndex.Value, nextTagStartIndex.Value + 4) == "<!--")
                {
                    var commentEndIndex = _html.IndexOfOrDefault("-->", nextTagStartIndex.Value + 4)
                      ?? throw new System.Exception("Comment didn't end");

                    result = _html.Cut(nextTagStartIndex.Value + 4, commentEndIndex);
                    type = ENextType.Comment;
                    _positionIndex = commentEndIndex + 3;
                    return true;
                }
                else if (_html.Cut(nextTagStartIndex.Value, nextTagStartIndex.Value + 9) == "<!DOCTYPE")
                {
                    var doctypeEndIndex = _html.IndexOfOrDefault(">", nextTagStartIndex.Value + 9)
                      ?? throw new System.Exception("Doctype didn't end");

                    result = _html.Cut(nextTagStartIndex.Value + 10, doctypeEndIndex);
                    type = ENextType.DocType;
                    _positionIndex = doctypeEndIndex + 1;
                    return true;
                }
            }

            // tag start
            var nextTagEndIndex2 = _html.IndexOfOrDefault(">", nextTagStartIndex.Value)
              ?? throw new System.Exception("There were only half of tag!");
            type = _html[nextTagEndIndex2 - 1] == '/'
              ? ENextType.TagStartEnd
              : ENextType.TagStart;
            var tagFullDefinition = _html.Cut(nextTagStartIndex + 1, type == ENextType.TagStart ? nextTagEndIndex2 : nextTagEndIndex2 - 1);

            // script
            if (tagFullDefinition.ToLower().StartsWith("script"))
            {
                var endingTagIndex = _html.IndexOfOrDefault("</script>", nextTagEndIndex2)
                  ?? throw new System.Exception("Script was not ended");

                type = ENextType.Script;
                result = _html.Cut(nextTagEndIndex2 + 1, endingTagIndex);
                _positionIndex = endingTagIndex + "</script>".Length;
                return true;
            }

            // not pair tag
            if (UnpairedTags.Contains(tagFullDefinition.Split(" ")[0].ToLower()))
                type = ENextType.TagStartEnd;

            result = tagFullDefinition;
            _positionIndex = nextTagEndIndex2 + 1;
            return true;
        }

        private Element CreateTag(string tagString, Element? parent)
        {
            var tagMatch = Regex.Match(tagString, "^(\\S+)\\s?");
            var tagName = tagMatch.Groups[1].Value;

            var result = new Element(parent, tagName);

            var attributes = Regex.Matches(tagString, "(\\S+)=('[^']*'|\"[^\"]*\")");
            attributes
              .Select(a => new { Name = a.Groups[1].Value, Value = a.Groups[2].Value.Cut(1, -1) })
              .ForEach(a => result.SetAttribute(a.Name, a.Value));

            var codeAttributes = Regex.Matches(tagString, "(\\S+)=(\\{[^\\}]*\\})");
            codeAttributes
              .Select(a => new { Name = a.Groups[1].Value, Value = a.Groups[2].Value.Cut(1, -1) })
              .ForEach(a => result.CodeAttributes.Add(a.Name, a.Value));

            return result;
        }

        public static string[] UnpairedTags = new string[] { "meta", "link", "input", "img", "br", "hr" };

        internal enum ENextType
        {
            DocType,
            Content,
            TagStart,
            TagEnd,
            TagStartEnd,
            Script,
            Comment
        }
    }
}