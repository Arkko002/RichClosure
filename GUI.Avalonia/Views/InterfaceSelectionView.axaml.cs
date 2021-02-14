using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for InterfaceSelectionWindow.xaml
    /// </summary>
    public partial class InterfaceSelectionView : ReactiveWindow<InterfaceSelectionViewModel>
    {
        public DataGrid AdapterDataGrid => this.FindControl<DataGrid>("AdapterDataGrid");

        public InterfaceSelectionView()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel,
                        model => model.NetworkInterfaces,
                        view => view.AdapterDataGrid.Items)
                    .DisposeWith(disposable);

                this.Bind(ViewModel,
                        model => model.SelectedInterface,
                        view => view.AdapterDataGrid.SelectedItem)
                    .DisposeWith(disposable);

            });

            AvaloniaXamlLoader.Load(this);
        }
    }
}
