using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for InterfaceSelectionWindow.xaml
    /// </summary>
    public partial class InterfaceSelectionWindow : ReactiveWindow<InterfaceSelectionViewModel>
    {
        public DataGrid NetworkDeviceDataGrid => this.FindControl<DataGrid>("NetworkDeviceDataGrid");
        public Button OkButton => this.FindControl<Button>("OkButton");
        public Button CancelButton => this.FindControl<Button>("CancelButton");

        public InterfaceSelectionWindow()
        {
            #if DEBUG
            this.AttachDevTools();
            #endif
            
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel,
                        model => model.NetworkInterfaces,
                        view => view.NetworkDeviceDataGrid.Items)
                    .DisposeWith(disposable);

                this.Bind(ViewModel,
                        model => model.SelectedInterface,
                        view => view.NetworkDeviceDataGrid.SelectedItem)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel!, vm => vm.OkCommand,
                    view => view.OkButton)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel!, vm => vm.CancelCommand,
                    view => view.CancelButton)
                    .DisposeWith(disposable);

                ViewModel!.OkCommand.Subscribe(Close).DisposeWith(disposable); 
                ViewModel!.CancelCommand.Subscribe(_ => Close()).DisposeWith(disposable);
            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
