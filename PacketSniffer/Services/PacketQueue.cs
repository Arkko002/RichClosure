using System.Collections.Concurrent;
using System.Threading;

namespace PacketSniffer.Services
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
