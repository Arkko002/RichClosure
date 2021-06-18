using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PacketSniffer.Factories;
using PacketSniffer.Packets;
using PacketSniffer.Socket;
using SharpPcap;

namespace PacketSniffer
{
    public class PacketSnifferPcap : IPcapSniffer
    {

        public ObservableCollection<IPacketFrame> Packets { get; }
        private readonly BlockingCollection<byte[]> _packetQueue;
        private readonly IAbstractFrameFactory _frameFactory;

        public ICaptureDevice SelectedDevice { get; set; }

        private readonly Task _dequeueTask;
        private readonly CancellationTokenSource _tokenSource;
        
        public PacketSnifferPcap()
        {
            _packetQueue = new BlockingCollection<byte[]>(new ConcurrentQueue<byte[]>());
            _frameFactory = new PacketFactory();
            Packets = new ObservableCollection<IPacketFrame>();

            _tokenSource = new CancellationTokenSource();
            _dequeueTask = new Task(DequeuePacketBuffer, _tokenSource.Token);
        }
        
        public CaptureDeviceList GetAvailableDevices()
        {
            return CaptureDeviceList.Instance;
        }

        public void SetCaptureDevice(int id)
        {
            this.SelectedDevice = CaptureDeviceList.Instance[id];
        }

        public void SniffPackets(ICaptureDevice device)
        {
            SelectedDevice = device;
            
            if (!SelectedDevice.Started)
            {
                SelectedDevice.Open();
                SelectedDevice.OnPacketArrival += this.OnPacketCapture;
                SelectedDevice.StartCapture();
                
                _dequeueTask.Start();
            }
        }

        public void StopSniffing()
        {
            if (SelectedDevice.Started)
            {
                SelectedDevice.StopCapture();
                SelectedDevice.Close();
                
                _tokenSource.Cancel();
                _packetQueue.Dispose();
            }
        }

        private void OnPacketCapture(object s, PacketCapture e)
        {
            _packetQueue.Add(e.Data.ToArray());
        }

        private void DequeuePacketBuffer()
        {
            var buffer = _packetQueue.Take();

            IPacketFrame packet = _frameFactory.CreatePacketFrame(buffer);
            Packets.Add(packet);
        }
    }
}