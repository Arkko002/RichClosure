using System;
using PacketSniffer.Factories;
using PacketSniffer.Services;
using PacketSniffer.Socket;
using richClosure.Avalonia.ViewModels;
using Splat;
using PacketSniffer = PacketSniffer.Services.PacketSniffer;

namespace richClosure.Avalonia
{
    public class AppBootstrapper : IEnableLogger
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new PacketDataGridViewModel());
            Locator.CurrentMutable.RegisterConstant(new global::PacketSniffer.Services.PacketSniffer(new PacketQueue(), new PacketFactory()), typeof(IPacketSniffer));
        }
    }
}