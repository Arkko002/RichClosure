using System.Collections.Generic;

namespace PacketFilter
{
    //TODO Make this use regular expressions
    public class SearchStringParser
    {
        public SearchQueryList ParseString(string commandString)
        {
            SearchQueryList searchQueries = new SearchQueryList();

            if (!commandString.Contains("|") && !commandString.Contains("&"))
            {
                searchQueries.OrQueries.Add(CreateSearchQuery(commandString));
                return searchQueries;
            }

            if (commandString.Contains("|"))
            {
                searchQueries.OrQueries = ParseOrString(commandString);
            }

            if (commandString.Contains("&"))
            {
                searchQueries.AndQueries = ParseAndString(commandString);
            }

            return searchQueries;
        }

        private List<SearchQuery> ParseOrString(string commandString)
        {
            string[] parsedOrConditions = commandString.Split('|');
            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedOrConditions)
            {
                searchQueries.Add(CreateSearchQuery(op));
            }

            return searchQueries;
        }

        private List<SearchQuery> ParseAndString(string commandString)
        {
            string[] parsedAndConditions = commandString.Split('&');
            List<SearchQuery> searchQueries = new List<SearchQuery>();

            foreach (var op in parsedAndConditions)
            {
                searchQueries.Add(CreateSearchQuery(op));
            }

            return searchQueries;
        }

        private SearchQuery CreateSearchQuery(string parsedString)
        {
            string[] queryParts = parsedString.TrimStart().TrimEnd().Split(' ');

            SearchQuery query = new SearchQuery
            {
                SearchedProperty = queryParts[0],
                OperatorStr = queryParts[1],
                SearchedValue = queryParts[2]
            };

            return query;
        }
    }
}
