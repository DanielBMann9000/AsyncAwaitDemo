using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AsyncAwaitDemo.Async.Traditional
{
    public struct FileReadProgressReport
    {
        public int? Bytes { get; set; }
        public string StatusText { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker bgw = new BackgroundWorker {WorkerReportsProgress = true};

        public MainWindow()
        {
            InitializeComponent();

            bgw.DoWork += (o, args) =>
            {
                var textFiles = Directory.EnumerateFiles(@".\TextFiles", "*.txt").ToList();
                
                bgw.ReportProgress(0, new FileReadProgressReport { StatusText = string.Format("Started processing {0} files..." + Environment.NewLine, textFiles.Count) });
                
                var totalFileLength = SumFiles(textFiles);
                var totalLength = totalFileLength;
                
                bgw.ReportProgress(0, new FileReadProgressReport { StatusText = string.Format(Environment.NewLine + "Done! Total length is: {0} bytes" + Environment.NewLine, totalLength) });
            };

            bgw.ProgressChanged += (o, args) =>
            {
                var report = (FileReadProgressReport)args.UserState;
                if (report.Bytes != null)
                {
                    byteSumBox.Text = (int.Parse(byteSumBox.Text) + report.Bytes.Value).ToString();
                }

                if (report.StatusText != null)
                {
                    status.Text += report.StatusText;
                }
            };
        
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            status.Text = string.Empty;
            byteSumBox.Text = "0";

            bgw.RunWorkerAsync();
            
            status.Text += "Control came back to Button_Click_1" + Environment.NewLine;
        }

        private int SumFiles(IEnumerable<string> textFiles)
        {

            int sum = 0;
            foreach (var file in textFiles)
            {
                sum += GetFileLength(file);
                Thread.Sleep(500);
            }

            return sum;
        }

        private int GetFileLength(string textFile)
        {
            bgw.ReportProgress(0, new FileReadProgressReport { StatusText = string.Format("Opening {0}" + Environment.NewLine, textFile) });
            
            using (var sr = new StreamReader(textFile))
            {
                var fileContents = sr.ReadToEnd();

                bgw.ReportProgress(0, new FileReadProgressReport { Bytes = fileContents.Length, StatusText = string.Format("Read {0}. Length was {1}" + Environment.NewLine, textFile, fileContents.Length) });

                return fileContents.Length;
            }
        }  
    }
}
