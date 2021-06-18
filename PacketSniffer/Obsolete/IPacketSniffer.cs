using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using PacketSniffer.Packets;

namespace PacketSniffer
{
    //TODO
    public interface IPacketSniffer
    {
        ObservableCollection<IPacketFrame> Packets { get; }
        IObservable<NetworkInterface> SelectedNetworkInterface { get; set; }

        IEnumerable<NetworkInterface> GetAvailableNetworkInterfaces();
        void SniffPackets(NetworkInterface networkInterface);
        void StopSniffing();
    }
}