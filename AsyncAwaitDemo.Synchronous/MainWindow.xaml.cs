using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AsyncAwaitDemo.Synchronous
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            status.Text = string.Empty;
            byteSumBox.Text = "0";

            var textFiles = Directory.EnumerateFiles(@".\TextFiles", "*.txt").ToList();
            
            status.Text += string.Format("Started processing {0} files..." + Environment.NewLine, textFiles.Count);
            
            var totalFileLength = SumFiles(textFiles);
            
            status.Text += "Control came back to Button_Click_1" + Environment.NewLine;
            
            var totalLength = totalFileLength;
            status.Text += string.Format(Environment.NewLine + "Done! Total length is: {0} bytes" + Environment.NewLine, totalLength);
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
            status.Text += string.Format("Opening {0}" + Environment.NewLine, textFile);

            using (var sr = new StreamReader(textFile))
            {
                var fileContents = sr.ReadToEnd();
                
                byteSumBox.Text = (int.Parse(byteSumBox.Text) + fileContents.Length).ToString();
                
                status.Text += string.Format("Read {0}. Length was {1}" + Environment.NewLine, textFile, fileContents.Length);
                
                return fileContents.Length;
            }
        }   
    }

}




