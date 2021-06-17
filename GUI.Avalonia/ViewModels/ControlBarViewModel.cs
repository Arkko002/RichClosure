using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;

namespace richClosure.Avalonia.ViewModels
{
    public class ControlBarViewModel : ViewModelBase
    {
       public ReactiveCommand<Unit, Unit> ShowInterfaceCommand { get; }
       public ReactiveCommand<Unit, Unit> StopSniffingCommand { get; }
       public Interaction<InterfaceSelectionViewModel, NetworkInterface?> ShowInterfaceInteraction { get; }
       
       public Window ParentWindow { get; }

       public ControlBarViewModel(ReactiveCommand<Unit, Unit> stopSniffingCommand, Window parentWindow)
       {
           StopSniffingCommand = stopSniffingCommand;
           ParentWindow = parentWindow;
           
           ShowInterfaceInteraction = new Interaction<InterfaceSelectionViewModel, NetworkInterface?>();

           ShowInterfaceCommand = ReactiveCommand.CreateFromTask(async () =>
           { 
               var vm = new InterfaceSelectionViewModel();

                var result = await ShowInterfaceInteraction.Handle(vm);

                if (result is not null)
                {
                }
            });
       }
    }
}