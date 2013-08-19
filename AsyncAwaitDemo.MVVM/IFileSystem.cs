using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncAwaitDemo.MVVM
{
    public interface IFileSystem
    {
        IEnumerable<string> EnumerateFiles(string folder, string fileMask);
        Task<string> GetFileContents(string fileName);
    }
}