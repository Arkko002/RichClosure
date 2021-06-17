using SharpPcap;

//TODO Create an interface for network devices to merge NetworkInterface and CaptureDevice
namespace PacketSniffer
{
    public interface IPcapSniffer
    {
        public ICaptureDevice SelectedDevice { get; set; }
        
        public CaptureDeviceList GetAvailableDevices();
        public void SetCaptureDevice(int id);
        
        public void SniffPackets();
        public void StopSniffing();
    }
}