using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;
using ReactiveUI;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketDataGridViewModel : ViewModelBase
    {
        public ObservableCollection<PacketViewModel> PacketViewModels { get; private set; }

        private PacketViewModel _selectedPacket;

        public PacketViewModel SelectedPacket
        {
            get => _selectedPacket;
            set => this.RaiseAndSetIfChanged(ref _selectedPacket, value);
        }
        
        public ObservableCollection<DataGridColumn> DataGridColumns { get; private set; }

        public PacketDataGridViewModel()
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(disposable =>
            {
                DataGridColumns = GenerateDataGridColumns();

                Disposable.Create(() => { }).DisposeWith(disposable);
            });
        }
        
        private ObservableCollection<DataGridColumn> GenerateDataGridColumns()
        {
            var columnsCollection = new ObservableCollection<DataGridColumn>
            {
                new DataGridTextColumn {Header = "ID", Binding = new Binding("Id")},
                new DataGridTextColumn {Header = "Time Captured", Binding = new Binding("DateTimeCaptured")},
                new DataGridTextColumn {Header = "Protocol", Binding = new Binding("Protocol")},
                new DataGridTextColumn {Header = "Dest. Addr.", Binding = new Binding("DestAddr")},
                new DataGridTextColumn {Header = "Src. Addr.", Binding = new Binding("SrcAddr")},
                new DataGridTextColumn {Header = "Dest. Port", Binding = new Binding("DestPort")},
                new DataGridTextColumn {Header = "Src. Port", Binding = new Binding("SrcPort")},
                new DataGridTextColumn {Header = "Comment", Binding = new Binding("Comment")}
            };

            return columnsCollection;
        }
    }
}