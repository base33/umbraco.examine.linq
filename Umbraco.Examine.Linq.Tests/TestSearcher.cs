using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.Tests
{
    public class TestSearcher : ISearcher
    {

        public IEnumerable<SearchResult> Search(string query, int skip, int take)
        {
            return new List<SearchResult>();
        }
    }
}
