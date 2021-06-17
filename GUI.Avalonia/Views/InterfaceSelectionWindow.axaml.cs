using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for InterfaceSelectionWindow.xaml
    /// </summary>
    public partial class InterfaceSelectionView : ReactiveWindow<InterfaceSelectionViewModel>
    {
        public DataGrid NetworkDeviceDataGrid => this.FindControl<DataGrid>("NetworkDeviceDataGrid");

        public InterfaceSelectionView()
        {
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

            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
