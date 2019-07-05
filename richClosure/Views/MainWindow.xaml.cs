using System.Threading;
using System.Windows;
using System.Windows.Data;
using richClosure.Packets;
using richClosure.Packets.Internet_Layer;
using richClosure.Packets.Transport_Layer;
using richClosure.ViewModels;

namespace richClosure.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        readonly object _packetListLockObject = new object();
        
        public MainWindow(MainWindowViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            Closed += (s, e) => _tokenSource.Cancel();

            // TODO MVVM multi-threading
            BindingOperations.EnableCollectionSynchronization(vm.PacketCollectionViewModel.PacketObservableCollection, _packetListLockObject);
        }
    }
}
