using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using PacketSniffer.Packets;
using ReactiveUI;

namespace richClosure.Avalonia.ViewModels
{
    //TODO Create a proper bit viewer for ascii and hex data
    //TODO Refrence to source packet breaks threading, find a way to copy/reference packet's data on UI
    public class PacketViewModel : ViewModelBase
    {
        public PacketTreeViewModel TreeViewModel { get; }
        public PacketHexViewModel HexViewModel { get; }
        public PacketGridViewModel GridViewModel { get; }
        
        public PacketViewModel(IPacket sourcePacket)
        {
            TreeViewModel = new PacketTreeViewModel(sourcePacket);
            HexViewModel = new PacketHexViewModel(sourcePacket);
            GridViewModel = new PacketGridViewModel(sourcePacket);
        }
    }
}
