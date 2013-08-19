using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncAwaitDemo.Asynchronous
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            beginButton.IsEnabled = false;
            status.Text = string.Empty;
            byteSumBox.Text = "0";

            var textFiles = Directory.EnumerateFiles(@".\TextFiles", "*.txt").ToList();

            status.Text += string.Format("Started processing {0} files..." + Environment.NewLine, textFiles.Count);

            var sumTask = SumFilesAsync(textFiles);

            status.Text += "Control came back to Button_Click_1" + Environment.NewLine;

            await sumTask;
            beginButton.IsEnabled = true;
            var totalLength = sumTask.Result;
            status.Text += string.Format(Environment.NewLine + "Done! Total length is: {0} bytes" + Environment.NewLine, totalLength);
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
            status.Text += string.Format("Opening {0}" + Environment.NewLine, textFile);

            using (var sr = new StreamReader(textFile))
            {
                var fileContents = await sr.ReadToEndAsync();

                byteSumBox.Text = (int.Parse(byteSumBox.Text) + fileContents.Length).ToString();

                status.Text += string.Format("Read {0}. Length was {1}" + Environment.NewLine, textFile, fileContents.Length);

                return fileContents.Length;
            }
        }
    }
}




