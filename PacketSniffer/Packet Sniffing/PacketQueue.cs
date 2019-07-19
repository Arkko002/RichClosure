using System.Collections.Concurrent;
using System.Threading;

namespace PacketSniffer.Packet_Sniffing
{
    public class PacketQueue
    {
        private readonly ConcurrentQueue<byte[]> _packetQueue = new ConcurrentQueue<byte[]>();
        private readonly AutoResetEvent _queueNotifier = new AutoResetEvent(false);

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
