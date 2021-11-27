using System.Text;

namespace Vue.cs.Generator.DomElements
{
    public class Text : INode
    {
        public Text(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public void Generate(StringBuilder sb)
        {
            sb.AppendLine($".AddText(\"{Content}\")");
        }
    }
}