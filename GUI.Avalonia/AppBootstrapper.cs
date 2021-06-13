using PacketSniffer;
using PacketSniffer.Factories;
using PacketSniffer.Socket;
using richClosure.Avalonia.ViewModels;
using Splat;

namespace richClosure.Avalonia
{
    public class AppBootstrapper : IEnableLogger
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new PacketDataGridViewModel());
            Locator.CurrentMutable.RegisterConstant(new global::PacketSniffer.PacketSniffer(), typeof(IPacketSniffer));
        }
    }
}
