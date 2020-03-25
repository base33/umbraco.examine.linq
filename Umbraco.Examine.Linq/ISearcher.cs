using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq
{
    public interface ISearcher
    {
        IEnumerable<SearchResult> Search(string query, int skip, int take, string orderByField, bool orderByAsc);
    }
}
