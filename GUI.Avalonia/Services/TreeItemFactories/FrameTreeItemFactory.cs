using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;
using richClosure.Avalonia.Models;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class FrameTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(PacketWrapper packet)
        {
            return new TreeViewItem { Header = "Frame " + packet.Id + ", Time Captured: " + packet };
        }
    }
}