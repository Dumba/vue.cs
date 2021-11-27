using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vue.cs.Generator.Expansions;

namespace Vue.cs.Generator.DomElements
{
    public class Element : INode
    {
        public Element(Element? parent = null, string tagName = "div")
        {
            Parent = parent;
            TagName = tagName;
        }

        // html attributes
        public Element? Parent { get; }
        public List<INode> Children { get; } = new();
        public string TagName { get; }
        public string[] Classes { get; private set; } = new string[0];
        public Dictionary<string, string> CustomAttributes { get; } = new();
        public Dictionary<string, string> CodeAttributes { get; } = new();

        public virtual void SetAttribute(string attribute, string value)
        {
            if (attribute == "class")
                Classes = value.Split(" ");

            else
                CustomAttributes.Add(attribute, value);
        }

        public override string ToString()
        {
            var idIdentify = CustomAttributes.TryGetValue("id", out var id)
              ? $"#{id}"
              : "";
            var classIndentify = Classes.Length > 0
              ? string.Join("", Classes.Select(c => $".{c}"))
              : "";
            return $"{TagName}{idIdentify}{classIndentify}";
        }

        public void Generate(StringBuilder sb)
        {
            // node or collection
            if (CodeAttributes.TryGetValue("v-for", out var forDefinition))
            {
                var splitted = forDefinition.Split(" in ");
                var param = splitted[0];
                var collectionName = splitted[1];

                sb.AppendLine($".AddChildren({collectionName}, \"{TagName}\", (b, {param}) => b");
            }
            else if (char.IsUpper(TagName[0]))
                sb.AppendLine($".AddChild<{TagName}>(b => b");
            else
                sb.AppendLine($".AddChild(\"{TagName}\", b => b");

            // classes
            foreach (var className in Classes)
            {
                sb.AppendLine($".AddClass(\"{className}\")");
            }

            // attributes
            foreach (var attribute in CustomAttributes)
            {
                sb.AppendLine($".AddAttribute(\"{attribute.Key}\", \"{attribute.Value}\")");
            }

            // conditions, events & reactiveValue
            foreach (var attribute in CodeAttributes)
            {
                if (attribute.Key == "v-if")
                    sb.AppendLine($".SetCondition({attribute.Value})");
                else if (attribute.Key == "v-for")
                {
                    // ignore, already done
                }
                else if (attribute.Key.StartsWith("@"))
                {
                    var i = attribute.Value.IndexOf("(");
                    var methodName = i >= 0
                        ? attribute.Value.Cut(null, i)
                        : attribute.Value;

                    var methodArgs = i >= 0
                        ? $", {attribute.Value.Cut(i + 1, -1)}"
                        : "";

                    sb.AppendLine($".AddEventListener(\"{attribute.Key.Substring(1)}\", \"{methodName}\"{methodArgs})");
                }
                else
                    sb.AppendLine($".AddAttribute(\"{attribute.Key}\", {attribute.Value})");
            }

            // children
            foreach (var child in Children)
            {
                child.Generate(sb);
            }

            sb.AppendLine(")");
        }
    }
}