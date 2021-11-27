using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vue.cs.Generator.DomElements;
using Vue.cs.Generator.Expansions;

namespace Vue.cs.Generator.Workers
{
    public class CodeGen
    {
        public string? Generate(IEnumerable<INode> nodes)
        {
            var root = nodes.FirstOrDefault(n => n is Element el && el.TagName == "template") as Element;
            var code = nodes.FirstOrDefault(n => n is Script) as Script;
            
            if (root is null)
                return code?.Content;

            (var start, var end) = SplitCode(code);

            var sb = new StringBuilder();

            sb.Append(start);
            GenerateTemplate(sb, root);
            sb.Append(end);

            return sb.ToString();
        }

        public (string start, string end) SplitCode(Script? code)
        {
            if (code is null)
                return ("", "");

            var classI = code.Content.IndexOf(" class ");
            var classBodyI = code.Content.IndexOf("{", classI);

            var start = code.Content.Cut(null, classBodyI + 1);
            var end = code.Content.Cut(classBodyI + 1);

            return (start, end);
        }

        private void GenerateTemplate(StringBuilder sb, Element root)
        {
            sb.AppendLine("public override void Setup(Builder builder, IEnumerable<IPageItem>? childNodes = null)");
            sb.AppendLine("{");
            
            if (root.Children.Any())
                GenerateBody(sb, root.Children);

            sb.AppendLine("}");
        }

        private void GenerateBody(StringBuilder sb, IEnumerable<INode> nodes)
        {
            sb.AppendLine("builder");
            
            foreach (var node in nodes)
            {
                node.Generate(sb);
            }
            
            sb.AppendLine(";");
        }
    }
}