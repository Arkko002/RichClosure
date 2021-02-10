using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    public class PacketTreeView : ReactiveWindow<PacketTreeViewModel>
    {
        public PacketTreeView()
        {
            //TODO
            this.WhenActivated(disposable => { });
            AvaloniaXamlLoader.Load(this);
        }
        
    }
}