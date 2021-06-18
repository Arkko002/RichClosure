using PacketDotNet;
using SharpPcap;
using Swordfish.NET.Collections;

//TODO Create an interface for network devices to merge NetworkInterface and CaptureDevice
namespace PacketSniffer
{
    public interface IPcapSniffer
    {
        ConcurrentObservableCollection<Packet> Packets { get; }
        public ICaptureDevice SelectedDevice { get; set; }
        
        public CaptureDeviceList GetAvailableDevices();
        public void SetCaptureDevice(int id);
        
        public void SniffPackets(ICaptureDevice device);
        public void StopSniffing();
    }
}