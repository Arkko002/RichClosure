using GUI.WPF.ViewModels;

namespace GUI.WPF.Views
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
