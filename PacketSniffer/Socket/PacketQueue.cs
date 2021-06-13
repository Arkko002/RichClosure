using System.Collections.Concurrent;
using System.Threading;

namespace PacketSniffer.Socket
{
    public class PacketQueue : IPacketQueue
    {
        private readonly ConcurrentQueue<byte[]> _packetQueue = new();
        private readonly AutoResetEvent _queueNotifier = new(false);
        private object _lock;

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

        public void Clear()
        {
            _queueNotifier.Close();
            _packetQueue.Clear();
        }
    }
}
