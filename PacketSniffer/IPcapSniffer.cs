using SharpPcap;

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