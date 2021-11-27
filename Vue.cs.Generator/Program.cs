using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vue.cs.Generator.Workers;

namespace Vue.cs.Generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            foreach (var arg in args.Where(a => Directory.Exists(a)).Select(d => Path.GetFullPath(d)))
            {
                foreach (var file in Tracer.FindVueCsFiles(arg))
                {
                    var content = await File.ReadAllTextAsync(file);
                    var p = new Parser(content);
                    var nodes = p.Parse();

                    var generator = new CodeGen();
                    var code = generator.Generate(nodes);

                    var newFilePath = Path.Combine(Path.GetDirectoryName(file)!, $"{Path.GetFileNameWithoutExtension(file)}.cs");
                    await File.WriteAllTextAsync(newFilePath, code);
                }
            }
        }
    }
}
