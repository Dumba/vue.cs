using System.Text;

namespace Vue.cs.Generator.DomElements
{
    public class Script : INode
    {
        public Script(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public void Generate(StringBuilder sb, int intendation)
        {
            sb.Append(Content);
        }
    }
}