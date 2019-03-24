using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using richClosure.Packet_Filtering;

namespace richClosure
{
    class SearchStringParser
    {
        public static List<SearchQuery> ParseOrString(string commandString)
        {
            string[] parsedOrConditions = commandString.Split('|');
            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedOrConditions)
            {
                SearchQuery query = new SearchQuery()
                {
                    SearchedProperty = op[0].ToString(),
                    OperatorStr = op[1].ToString(),
                    SearchedValue = op[2].ToString()
                };

                searchQueries.Add(query);
            }

            return searchQueries;
        }

        public static List<SearchQuery> ParseAndString(string commandString)
        {
            string[] parsedAndConditions = commandString.Split('&');

            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedAndConditions)
            {
                SearchQuery query = new SearchQuery()
                {
                    SearchedProperty = op[0].ToString(),
                    OperatorStr = op[1].ToString(),
                    SearchedValue = op[2].ToString()
                };

                searchQueries.Add(query);
            }

            return searchQueries;
        }
    }
}
