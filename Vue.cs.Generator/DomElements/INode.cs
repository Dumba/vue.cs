using System.Text;

namespace Vue.cs.Generator.DomElements
{
    public interface INode
    {
        void Generate(StringBuilder sb, int intendation);
    }
}