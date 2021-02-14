using Avalonia.Controls;
using PacketSniffer.Packets;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class FrameTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacketFrame packet)
        {
            return new TreeViewItem { Header = "Frame " + packet.PacketId + ", Time Captured: " + packet.DateTimeCaptured };
        }
    }
}