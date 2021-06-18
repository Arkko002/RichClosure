using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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

                this.OneWayBind(ViewModel!, vm => vm.PacketSniffer.Packets,
                        view => view.PacketDataGrid.Items)
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
