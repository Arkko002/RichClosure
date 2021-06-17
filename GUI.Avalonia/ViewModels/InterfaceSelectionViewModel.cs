using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;

namespace richClosure.Avalonia.ViewModels
{
    public class InterfaceSelectionViewModel : ViewModelBase
    {
        public List<NetworkInterface> NetworkInterfaces { get; private set; }

        private NetworkInterface _selectedInterface;
        public NetworkInterface SelectedInterface
        {
            get => _selectedInterface;
            set => this.RaiseAndSetIfChanged(ref _selectedInterface, value);
        }
        
        public ReactiveCommand<Unit, NetworkInterface> Ok { get; private set; }
        public ReactiveCommand<Unit, Unit> Cancel { get; private set; }

        public InterfaceSelectionViewModel()
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(disposable =>
            {
                
                var okEnabled = this.WhenAnyValue(x => x.SelectedInterface != null);
                Ok = ReactiveCommand.Create(() => SelectedInterface, okEnabled);
                Cancel = ReactiveCommand.Create(() => { });


                NetworkInterfaces = new List<NetworkInterface>();
                NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();

                Disposable
                    .Create(() => { })
                    .DisposeWith(disposable);
            });
        }

        public ViewModelActivator Activator { get; }
    }
}
