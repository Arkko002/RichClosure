using System.Threading;

namespace richClosure.Packet_Sniffing
{
    class SnifferThreads
    {
        private readonly Thread _enqueueThread;
        private readonly Thread _dequeueThread;

        public SnifferThreads(ThreadStart enqueueMethod, ThreadStart dequeueMethod)
        {
            _enqueueThread = new Thread(enqueueMethod);
            _dequeueThread = new Thread(dequeueMethod);
        }

        public void StartThreads()
        {
            _enqueueThread.Start();
            _dequeueThread.Start();
        }
    }
}
