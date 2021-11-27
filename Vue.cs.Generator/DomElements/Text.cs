using System.Text;
using Vue.cs.Generator.Expansions;

namespace Vue.cs.Generator.DomElements
{
    public class Text : INode
    {
        public Text(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public void Generate(StringBuilder sb, int intendation)
        {
            var content = Content;
            while (TryGetCode(content, out var before, out var inside, out content))
            {
                if (before is not null && before.Length > 0)
                    sb.Append(' ', intendation).AppendLine($".AddText(\"{before}\")");
                if (inside is not null && inside.Length > 0)
                    sb.Append(' ', intendation).AppendLine($".AddText({inside})");
            }

            if (content is not null && content.Length > 0)
                sb.Append(' ', intendation).AppendLine($".AddText(\"{content}\")");
        }

        private bool TryGetCode(string content, out string? before, out string? inside, out string rest)
        {
            var keyStart = "{{";
            var keyEnd = "}}";

            var iStart = content.IndexOf(keyStart);
            var iEnd = content.IndexOf(keyEnd, iStart < 0 ? 0 : iStart);
            if (iStart < 0 || iEnd < 0)
            {
                before = null;
                inside = null;
                rest = content.Trim();

                return false;
            }

            before = content.Cut(null, iStart).Trim();
            inside = content.Cut(iStart + keyStart.Length, iEnd).Trim();
            rest = content.Cut(iEnd + keyEnd.Length, null).Trim();
            return true;
        }
    }
}