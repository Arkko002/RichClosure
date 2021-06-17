using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Avalonia.Controls;
using PacketSniffer.Packets;
using ReactiveUI;
using richClosure.Avalonia.Services.TreeItemFactories;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketTreeViewModel : ViewModelBase
    {
        //TODO break PacketViewModel into submodels
        public ObservableCollection<TreeViewItem> TreeViewItems { get; private set; }

        private IPacketFrame _packetFrame;
        private IAbstractTreeItemFactory _treeItemFactory;
        
        public PacketTreeViewModel(IPacketFrame packetFrame)
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(disposable =>
            {
                //CreateTreeViewItems();
                
                _packetFrame = packetFrame;
                Disposable
                    .Create(() => { })
                    .DisposeWith(disposable);
            });
        }

        void CreateTreeViewItems()
        {
            var frameFactory = new FrameTreeItemFactory();
            var frameItem = frameFactory.CreateTreeViewItem(_packetFrame);
            
            TreeViewItems.Add(frameItem);

            IPacket packet = _packetFrame.Packet;
            while (packet.PreviousHeader != null)
            {
                TreeViewItems.Add(_treeItemFactory.CreateTreeViewItem(packet.PreviousHeader));
                packet = packet.PreviousHeader;
            }
        }
    }
}