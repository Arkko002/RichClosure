
namespace richClosure
{
    class ErrorLogger
    {

        public enum ErrorSeverity
        {
            Low,
            Medium,
            High
        };

        private static readonly object LockObject = new object();

        public static void LogError(string dateTime, string errorMsg, object location, ErrorSeverity severity, string dumpData)
        {
            lock(LockObject)
            {
                switch (severity)
                {
                    case ErrorSeverity.Low:
                        System.IO.File.AppendAllText("LogFile.txt", "AT " + dateTime + "\n");
                        System.IO.File.AppendAllText("LogFile.txt", errorMsg + " in " + location + "\n");

                        if(dumpData != string.Empty)
                        {
                            System.IO.File.AppendAllText("LogFile.txt", dumpData + "\n");
                        }

                        System.IO.File.AppendAllText("LogFile.txt", "---------END----------");
                        break;

                    case ErrorSeverity.Medium:

                        break;

                    case ErrorSeverity.High:

                        break;
                }
            }

        }
    }
}
