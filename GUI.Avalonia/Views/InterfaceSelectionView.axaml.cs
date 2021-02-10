using System.Reactive.Disposables;
using Avalonia;
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
    public partial class InterfaceSelectionWindow : ReactiveWindow<InterfaceSelectionViewModel>
    {
        public InterfaceSelectionWindow()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel,
                        model => model.NetworkInterfaces,
                        view => view.FindControl<DataGrid>("AdapterDataGrid").Items)
                    .DisposeWith(disposable);

                this.Bind(ViewModel,
                        model => model.SelectedInterface,
                        view => view.FindControl<DataGrid>("AdapterDataGrid").SelectedItem)
                    .DisposeWith(disposable);

            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
