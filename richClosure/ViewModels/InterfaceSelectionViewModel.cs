using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using richClosure.Commands;

namespace richClosure.ViewModels
{
    public class InterfaceSelectionViewModel : IViewModel
    {
        public List<NetworkInterface> NetworkInterfaces { get; set; }
        public NetworkInterface SelectedInterface { get; set; }

        public ICommand ChooseInterfaceCommand { get; set; }
        public ICommand CloseRequestCommand { get; set; }

        public EventHandler ClosingRequest;

        private PacketSnifferViewModel _packetSnifferViewModel;

        public InterfaceSelectionViewModel(PacketSnifferViewModel packetSnifferViewModel)
        {
            NetworkInterfaces = new List<NetworkInterface>();
            _packetSnifferViewModel = packetSnifferViewModel;

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
        }

        private void CloseSelectionWindow()
        {
            if (!(ClosingRequest is null))
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}
