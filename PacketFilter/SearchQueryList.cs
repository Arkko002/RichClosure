using System.Collections.Generic;

namespace Gui.PacketFilter
{
    public class SearchQueryList
    {
        public List<SearchQuery> OrQueries { get; set; }
        public List<SearchQuery> AndQueries { get; set; }

        public SearchQueryList()
        {
            OrQueries = new List<SearchQuery>();
            AndQueries = new List<SearchQuery>();
        }
    }
}
