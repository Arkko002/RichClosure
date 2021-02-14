using Avalonia.Controls;
using PacketSniffer.Packets;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public interface IAbstractTreeItemFactory
    {
        TreeViewItem CreateTreeViewItem(IPacket packet);
    }
}