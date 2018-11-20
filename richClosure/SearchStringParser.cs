using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace richClosure
{
    class SearchStringParser
    {
        public string[] ParseOrString(string commandString)
        {
            if (commandString.Contains('|'))
            {
                string[] parsedOrConditions = commandString.Split('|');

                return parsedOrConditions;
            }
            else
            {
                return null;
            }
        }

        public string[] ParseAndString(string commandString)
        {
            if (commandString.Contains('&'))
            {
                string[] parsedAndConditions = commandString.Split('&');

                return parsedAndConditions;
            }
            else
            {
                return null;
            }

        }
    }
}
