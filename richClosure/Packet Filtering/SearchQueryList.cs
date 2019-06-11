using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace richClosure.Packet_Filtering
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
