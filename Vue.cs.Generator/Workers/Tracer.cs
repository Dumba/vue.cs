using System.Collections.Generic;
using System.IO;

namespace Vue.cs.Generator.Workers
{
    public static class Tracer
    {
        public static IEnumerable<string> FindVueCsFiles(string directoryPath)
        {
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.vue-cs"))
            {
                yield return file;
            }

            foreach (var directoryChildPath in Directory.EnumerateDirectories(directoryPath))
            {
                foreach (var file in FindVueCsFiles(directoryChildPath))
                {
                    yield return file;
                }
            }
        }
    }
}