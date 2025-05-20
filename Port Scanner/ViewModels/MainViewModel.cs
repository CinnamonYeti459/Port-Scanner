using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Port_Scanner.Models;

namespace Port_Scanner.ViewModels
{
    public partial class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly PortScanner _portScanner = new();

        public ObservableCollection<PortScanResult> ScanResults => _portScanner.ScanResults;

        private int _scanProgress;
        public int ScanProgress
        {
            get => _scanProgress;
            set
            {
                if (_scanProgress != value)
                {
                    _scanProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task StartScanAsync(string host, int startPort, int endPort)
        {
            // Hook up progress reporting from PortScanner
            _portScanner.ProgressChanged = (progress) =>
            {
                ScanProgress = progress;
            };

            await _portScanner.ScanPortsAsync(host, startPort, endPort);
        }
    }
}