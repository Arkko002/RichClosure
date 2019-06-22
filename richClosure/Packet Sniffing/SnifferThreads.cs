using System.Threading;

namespace richClosure.Packet_Sniffing
{
    public class SnifferThreads
    {
        private readonly Thread _enqueueThread;
        private readonly Thread _dequeueThread;

        public SnifferThreads(ThreadStart enqueueMethod, ThreadStart dequeueMethod)
        {
            _enqueueThread = new Thread(enqueueMethod);
            _dequeueThread = new Thread(dequeueMethod);

            _enqueueThread.SetApartmentState(ApartmentState.STA);
            _dequeueThread.SetApartmentState(ApartmentState.STA);

            _enqueueThread.IsBackground = true;
            _dequeueThread.IsBackground = true;
        }

        public void StartThreads()
        {
            _enqueueThread.Start();
            _dequeueThread.Start();
        }
    }
}
