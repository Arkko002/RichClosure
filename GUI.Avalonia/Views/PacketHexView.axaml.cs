using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using richClosure.Avalonia.ViewModels;

namespace richClosure.Avalonia.Views
{
    public class PacketHexUserControl : ReactiveUserControl<PacketHexViewModel>
    {
        public PacketHexUserControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}