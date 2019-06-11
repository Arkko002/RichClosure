using System;
using System.Collections.Generic;

namespace richClosure.Packet_Filtering
{
    public class SearchStringParser
    {
        //TODO main public method, check for single commands, not only chained | or &
        public List<SearchQuery> ParseString()
        {
            throw new NotImplementedException();
        }

        public List<SearchQuery> ParseOrString(string commandString)
        {
            if (!commandString.Contains("|"))
            {
                return new List<SearchQuery>();
            }

            string[] parsedOrConditions = commandString.Split('|');
            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedOrConditions)
            {
                string[] queryParts = op.TrimStart().TrimEnd().Split(' ');

                SearchQuery query = new SearchQuery
                {
                    SearchedProperty = queryParts[0],
                    OperatorStr = queryParts[1],
                    SearchedValue = queryParts[2]
                };

                searchQueries.Add(query);
            }

            return searchQueries;
        }

        public List<SearchQuery> ParseAndString(string commandString)
        {
            if (!commandString.Contains("&"))
            {
                return new List<SearchQuery>();
            }

            string[] parsedAndConditions = commandString.Split('&');
            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedAndConditions)
            {
                string[] queryParts = op.TrimStart().TrimEnd().Split(' ');

                SearchQuery query = new SearchQuery
                {
                    SearchedProperty = queryParts[0],
                    OperatorStr = queryParts[1],
                    SearchedValue = queryParts[2]
                };

                searchQueries.Add(query);
            }

            return searchQueries;
        }
    }
}
