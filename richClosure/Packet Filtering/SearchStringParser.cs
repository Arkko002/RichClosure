using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace richClosure
{
    class SearchStringParser
    {
        public static string[] ParseOrString(string commandString)
        {
            string[] parsedOrConditions = commandString.Split('|');

            return parsedOrConditions;
        }

        public static string[] ParseAndString(string commandString)
        {
            string[] parsedAndConditions = commandString.Split('&');

            return parsedAndConditions;
        }

        public static string[] ParseStringSegments(string commandString)
        {
            string[] parsedString = commandString.Split(' ');

            return parsedString;
        }
    }
}
