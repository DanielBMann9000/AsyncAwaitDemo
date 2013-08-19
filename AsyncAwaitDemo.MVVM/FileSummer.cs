using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncAwaitDemo.MVVM
{
    public class FileSummer
    {
        public event EventHandler<string> StatusChanged;
        public event EventHandler<int> BytesChanged;

        private readonly IFileSystem fileSystem;

        public FileSummer(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public FileSummer() : this(new FileSystem())
        {   
        }

        public async Task<int> StartSumming()
        {
            var textFiles = fileSystem.EnumerateFiles(@".\TextFiles", "*.txt").ToList();

            FireStatusChangedEvent(string.Format("Started processing {0} files..." + Environment.NewLine, textFiles.Count));

            var sumTask = SumFilesAsync(textFiles);

            FireStatusChangedEvent("Control came back to Button_Click_1" + Environment.NewLine);

            await sumTask;

            return sumTask.Result;
        }

        private async Task<int> SumFilesAsync(IEnumerable<string> textFiles)
        {
            var tasks = new List<Task<int>>();
            foreach (var file in textFiles)
            {
                tasks.Add(GetFileLengthAsync(file));
                await Task.Delay(500);
            }

            await Task.WhenAll(tasks);
            return tasks.Sum(t => t.Result);
        }

        private async Task<int> GetFileLengthAsync(string textFile)
        {
            FireStatusChangedEvent(string.Format("Opening {0}" + Environment.NewLine, textFile));
            
            var fileContents = await fileSystem.GetFileContents(textFile);

            FireBytesChangedEvent(fileContents.Length);

            FireStatusChangedEvent(string.Format("Read {0}. Length was {1}" + Environment.NewLine, textFile, fileContents.Length));

            return fileContents.Length;
            
        }

        private void FireStatusChangedEvent(string text)
        {
            var handler = StatusChanged;
            if (handler != null)
            {
                handler(this, text);
            }
        }

        private void FireBytesChangedEvent(int bytes)
        {
            var handler = BytesChanged;
            if (handler != null)
            {
                handler(this, bytes);
            }
        }
    }
}
