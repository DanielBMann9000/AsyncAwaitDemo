using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncAwaitDemo.MVVM
{
    public class FileSystem : IFileSystem
    {
        public IEnumerable<string> EnumerateFiles(string folder, string fileMask)
        {
            return Directory.EnumerateFiles(folder, fileMask);
        }

        public async Task<string> GetFileContents(string fileName)
        {
            using (var stream = new StreamReader(fileName))
            {
                return await stream.ReadToEndAsync();
            }
        }
    }
}