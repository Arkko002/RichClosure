using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace richClosure.Packet_Sniffing
{
    //TODO Gracefully stop threads on request
    class PacketQueue
    {
        private ConcurrentQueue<byte[]> _packetQueue = new ConcurrentQueue<byte[]>();
        private AutoResetEvent _queueNotifier = new AutoResetEvent(false);

        public void EnqueuePacket(byte[] buffer)
        {
            _packetQueue.Enqueue(buffer);
            _queueNotifier.Set();
        }

        public byte[] DequeuePacket()
        {
            _queueNotifier.WaitOne();

            _packetQueue.TryDequeue(out var buffer);            
            return buffer;                        
        }
    }
}
