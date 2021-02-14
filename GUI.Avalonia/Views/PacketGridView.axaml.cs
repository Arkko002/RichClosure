using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    public class PacketGridUserControl : ReactiveUserControl<PacketDataGridViewModel>
    {
        public DataGrid PacketDataGrid => this.FindControl<DataGrid>("PacketDataGrid");
        
        public PacketGridUserControl()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel,
                    x => x.DataGridColumns,
                    x => x.PacketDataGrid.Columns)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel,
                    x => x.PacketViewModels,
                    x => x.PacketDataGrid.Items)
                    .DisposeWith(disposable);

                this.Bind(ViewModel,
                    x => x.SelectedPacket,
                    x => x.PacketDataGrid.SelectedItem)
                    .DisposeWith(disposable);
            });
            AvaloniaXamlLoader.Load(this);
        }

    }
}