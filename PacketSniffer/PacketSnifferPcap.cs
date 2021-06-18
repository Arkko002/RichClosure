using PacketDotNet;
using SharpPcap;
using Swordfish.NET.Collections;

namespace PacketSniffer
{
    public class PacketSnifferPcap : IPcapSniffer
    {

        public ConcurrentObservableCollection<Packet> Packets { get; }
        public ICaptureDevice SelectedDevice { get; set; }

        private int _id;

        
        public PacketSnifferPcap(ConcurrentObservableCollection<Packet> packets)
        {
            _id = 0;
            Packets = packets;
        }
        
        public CaptureDeviceList GetAvailableDevices()
        {
            return CaptureDeviceList.Instance;
        }

        public void SetCaptureDevice(int id)
        {
            SelectedDevice = CaptureDeviceList.Instance[id];
        }

        public void SniffPackets(ICaptureDevice device)
        {
            SelectedDevice = device;
            
            if (!SelectedDevice.Started)
            {
                SelectedDevice.Open();
                SelectedDevice.OnPacketArrival += OnPacketCapture;
                SelectedDevice.StartCapture();
            }
        }

        public void StopSniffing()
        {
            if (SelectedDevice.Started)
            {
                SelectedDevice.StopCapture();
                SelectedDevice.Close();
            }
        }

        private void OnPacketCapture(object s, PacketCapture e)
        {
            var rawPacket = e.GetPacket();
            var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            Packets.Add(packet);
        }
    }
}