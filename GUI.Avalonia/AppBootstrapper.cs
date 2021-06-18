using PacketDotNet;
using PacketSniffer;
using Splat;
using Swordfish.NET.Collections;

namespace richClosure.Avalonia
{
    public class AppBootstrapper : IEnableLogger
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new ConcurrentObservableCollection<Packet>());
            Locator.CurrentMutable.RegisterConstant(new PacketSnifferPcap(Locator.Current.GetService<ConcurrentObservableCollection<Packet>>()), typeof(IPcapSniffer));
            Locator.CurrentMutable.RegisterConstant(new PacketSniffer.PacketSniffer(), typeof(IPacketSniffer));
        }
    }
}
