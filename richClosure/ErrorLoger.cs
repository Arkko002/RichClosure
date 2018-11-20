
namespace richClosure
{
    class ErrorLoger
    {

        public enum ErrorSeverity
        {
            low,
            medium,
            high
        };

        private static object lockObject = new object();

        private ErrorLoger()
        {
        }

        public static void LogError(string dateTime, string errorMsg, object location, ErrorSeverity severity, string dumpData)
        {
            lock(lockObject)
            {
                switch (severity)
                {
                    case ErrorSeverity.low:
                        System.IO.File.AppendAllText("LogFile.txt", "AT " + dateTime + "\n");
                        System.IO.File.AppendAllText("LogFile.txt", errorMsg + " in " + location.ToString() + "\n");

                        if(dumpData != string.Empty)
                        {
                            System.IO.File.AppendAllText("LogFile.txt", dumpData + "\n");
                        }

                        System.IO.File.AppendAllText("LogFile.txt", "---------END----------");
                        break;

                    case ErrorSeverity.medium:

                        break;

                    case ErrorSeverity.high:

                        break;
                }
            }

        }
    }
}
