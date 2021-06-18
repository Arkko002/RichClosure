using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SharpPcap;

namespace richClosure.Avalonia.ViewModels
{
    public class InterfaceSelectionViewModel : ViewModelBase
    {
        public IObservableCollection<ICaptureDevice> NetworkInterfaces { get; private set; }

        private ICaptureDevice _selectedInterface;
        public ICaptureDevice SelectedInterface
        {
            get => _selectedInterface;
            set => this.RaiseAndSetIfChanged(ref _selectedInterface, value);
        }
        
        public ReactiveCommand<Unit, ICaptureDevice> OkCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; private set; }

        public InterfaceSelectionViewModel(CaptureDeviceList devices)
        {
            // var okEnabled = this.WhenAnyValue(x => x._selectedInterface != null);
            OkCommand = ReactiveCommand.Create(() => { return SelectedInterface; });
            CancelCommand = ReactiveCommand.Create(() => { });

            NetworkInterfaces = new ObservableCollectionExtended<ICaptureDevice>();
            NetworkInterfaces.AddRange(devices);
        }
    }
}
