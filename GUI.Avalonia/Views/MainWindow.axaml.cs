using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;
using SharpPcap;

namespace richClosure.Avalonia.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public Button StartSniffingButton => this.FindControl<Button>("StartButton");
        public Button StopSniffingButton => this.FindControl<Button>("StopButton");
        public DataGrid PacketDataGrid => this.FindControl<DataGrid>("PacketDataGrid");
        public TreeView PacketTreeView => this.FindControl<TreeView>("PacketTreeView");
        public TextBox PacketHexTextBox => this.FindControl<TextBox>("PacketHexTextBox");
        public TextBox PacketAsciiTextBox => this.FindControl<TextBox>("PacketAsciiTextBox");
        public Label TotalPacketsLabel => this.FindControl<Label>("TotalPacketsLabelNum");
        public Label ShownPacketsLabel => this.FindControl<Label>("ShownPacketsLabelNum");
        
        
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            
            #if DEBUG
            this.AttachDevTools();
            #endif
            
            this.WhenActivated(d =>
            {
                ViewModel!.ShowInterfaceInteraction.RegisterHandler(DoShowDialogAsync).DisposeWith(d);
                
                this.BindCommand(ViewModel!, vm => vm.ShowInterfaceCommand,
                        view => view.StartSniffingButton)
                    .DisposeWith(d);
            
                this.BindCommand(ViewModel!, vm => vm.StopSniffingCommand,
                        view => view.StopSniffingButton)
                    .DisposeWith(d);

                // this.OneWayBind(ViewModel!, vm => vm.Packets.Count,
                //     view => view.TotalPacketsLabel.Content)
                //     .DisposeWith(d);
                //TODO Shown packets count, filtering

                this.Bind(ViewModel!, vm => vm.SelectedPacket,
                    view => view.PacketDataGrid.SelectedItem)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel!, vm => vm.SelectedPacket.HexString,
                        view => view.PacketHexTextBox.Text)
                    .DisposeWith(d);
                
                this.OneWayBind(ViewModel!, vm => vm.SelectedPacket.AsciiString,
                        view => view.PacketAsciiTextBox.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel!, vm => vm.SelectedPacket.PacketTreeItems,
                        view => view.PacketTreeView.Items)
                    .DisposeWith(d);

            });
        }
        
        private async Task DoShowDialogAsync(InteractionContext<InterfaceSelectionViewModel, ICaptureDevice?> interaction)
        {
            var dialog = new InterfaceSelectionWindow
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<ICaptureDevice>(this);
            interaction.SetOutput(result);
        }
    }
}
