using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace richClosure
{
    class SearchStringParser
    {
        public static (string property, string comparisionOp, string value)? ParseOrString(string commandString)
        {
            string[] parsedOrConditions = commandString.Split('|');

            return (parsedOrConditions[0], parsedOrConditions[1], parsedOrConditions[2]);
        }

        public static (string property, string comparisionOp, string value)? ParseAndString(string commandString)
        {
            string[] parsedAndConditions = commandString.Split('&');
        
            return (parsedAndConditions[0], parsedAndConditions[1], parsedAndConditions[2]);
        }

        public static (string property, string comparisionOp, string value)? ParseStringSegments(string commandString)
        {
            string[] parsedString = commandString.Split(' ');

            return (parsedString[0], parsedString[1], parsedString[2]);
        }
    }
}
