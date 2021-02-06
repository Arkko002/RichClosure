using GUI.WPF.Commands;
using GUI.WPF.Services.Window_Services;

namespace GUI.WPF.ViewModels
{
    public class InterfaceSelectionViewModel : IViewModel
    {
        public List<NetworkInterface> NetworkInterfaces { get; set; }
        public NetworkInterface SelectedInterface { get; set; }

        public ICommand ChooseInterfaceCommand { get; set; }
        public ICommand CloseRequestCommand { get; set; }

        public IWindowManager WindowManager { get; set; }

        private readonly PacketSnifferViewModel _packetSnifferViewModel;

        //TODO Replace ViewModel dependency with the properties needed in this VM
        public InterfaceSelectionViewModel(PacketSnifferViewModel packetSnifferViewModel, IWindowManager windowManager)
        {
            NetworkInterfaces = new List<NetworkInterface>();
            _packetSnifferViewModel = packetSnifferViewModel;

            WindowManager = windowManager;

            ChooseInterfaceCommand = new RelayCommand(x => ChoseInterface(), x => true);
            CloseRequestCommand = new RelayCommand(x => CloseSelectionWindow(), x => true);

            PopulateInterfaceList();
        }

        private void PopulateInterfaceList()
        {
            NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();
        }

        private void ChoseInterface()
        {
            _packetSnifferViewModel.NetworkInterface = SelectedInterface;
            if (_packetSnifferViewModel.StartSniffingCommand.CanExecute(null))
            {
                _packetSnifferViewModel.StartSniffingCommand.Execute(null);
            }
            CloseSelectionWindow();
        }

        private void CloseSelectionWindow()
        {
            // TODO Close only caller window
            WindowManager.CloseWindow();
        }
    }
}
