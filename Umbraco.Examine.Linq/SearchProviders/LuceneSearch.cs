using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.LuceneEngine.Providers;
using Examine.SearchCriteria;

namespace Umbraco.Examine.Linq.SearchProviders
{
    public class LuceneSearch : ISearcher
    {
        protected string IndexName { get; set; }

        public static Dictionary<string, ISearchCriteria> searchQueryCache { get; set; }

        static LuceneSearch()
        {
            searchQueryCache = new Dictionary<string, ISearchCriteria>();
        }

        public LuceneSearch(string indexName)
        {
            IndexName = indexName;
        }

        public IEnumerable<SearchResult> Search(string query, int skip, int take)
        {
            LuceneSearcher searcher = ExamineManager.Instance.SearchProviderCollection[IndexName] as LuceneSearcher;

            var results = searcher.Search(searcher.CreateSearchCriteria().RawQuery(query)).Skip(skip);

            if (take > -1)
                results = results.Take(take);
            return results.ToList();
        }
    }
}
