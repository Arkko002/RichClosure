using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PacketSniffer.Packets;

namespace richClosure.ViewModels.PacketViewModelFactories
{
    internal interface IPacketTreeViewItemFactory
    {
        TreeViewItem CreatePacketTreeViewItem(IPacket sourcePacket);
    }
}
