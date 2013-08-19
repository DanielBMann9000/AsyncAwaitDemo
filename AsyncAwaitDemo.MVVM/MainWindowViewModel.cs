using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitDemo.MVVM.Annotations;

namespace AsyncAwaitDemo.MVVM
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private int byteSum;
        private string statusText;
        private bool isProcessing;

        public int ByteSum
        {
            get
            {
                return this.byteSum;
            }
            set
            {
                if (this.byteSum != value)
                {
                    this.byteSum = value;
                    OnPropertyChanged();
                }
            }
        }
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand BeginButtonCommand { get; private set; }
        private readonly FileSummer summer;

        public MainWindowViewModel(IFileSystem fileSystem)
        {
            summer = new FileSummer(fileSystem);
            summer.BytesChanged += (sender, bytes) =>
            {
                this.ByteSum += bytes;
            };
            summer.StatusChanged += (sender, message) =>
            {
                StatusText += message;
            };
            this.BeginButtonCommand = new AsyncDelegateCommand(ExecuteBeginButtonCommand, CanExecuteBeginButtonCommand);
        }

        public MainWindowViewModel() : this(new FileSystem())
        {
        }

        private async Task ExecuteBeginButtonCommand(object o)
        {
            this.isProcessing = true;
            this.StatusText = string.Empty;
            this.ByteSum = 0;

            var totalLength = await summer.StartSumming();

            this.StatusText += string.Format(Environment.NewLine + "Done! Total length is: {0} bytes" + Environment.NewLine, totalLength);
            this.isProcessing = false;
        }

        private bool CanExecuteBeginButtonCommand(object o)
        {
            return !this.isProcessing;
        }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged
    }
}
