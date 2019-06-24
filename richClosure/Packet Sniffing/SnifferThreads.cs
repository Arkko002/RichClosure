using System.Threading;

namespace richClosure.Packet_Sniffing
{
    public class SnifferThreads
    {
        private Thread _enqueueThread;
        private Thread _dequeueThread;

        public void AssignMethodsToThreads(ThreadStart enqueueMethod, ThreadStart dequeueMethod)
        {
            _enqueueThread = new Thread(enqueueMethod);
            _dequeueThread = new Thread(dequeueMethod);

            ConfigureThreads();
        }

        private void ConfigureThreads()
        {
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
