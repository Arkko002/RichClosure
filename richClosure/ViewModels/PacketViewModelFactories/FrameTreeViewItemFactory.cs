using PacketSniffer.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace richClosure.ViewModels.PacketViewModelFactories
{
    class FrameTreeViewItemFactory : IPacketTreeViewItemFactory
    {
        public TreeViewItem CreatePacketTreeViewItem(IPacket sourcePacket)
        {
            return new TreeViewItem { Header = "Frame " + sourcePacket.PacketId + ", Time Captured: " + sourcePacket.TimeDateCaptured };
        }
    }
}
