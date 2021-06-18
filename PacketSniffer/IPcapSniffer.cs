using System.Collections.ObjectModel;
using PacketSniffer.Packets;
using SharpPcap;

//TODO Create an interface for network devices to merge NetworkInterface and CaptureDevice
namespace PacketSniffer
{
    public interface IPcapSniffer
    {
        ObservableCollection<IPacketFrame> Packets { get; }
        public ICaptureDevice SelectedDevice { get; set; }
        
        public CaptureDeviceList GetAvailableDevices();
        public void SetCaptureDevice(int id);
        
        public void SniffPackets(ICaptureDevice device);
        public void StopSniffing();
    }
}