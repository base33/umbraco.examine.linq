using Examine;
using Examine.SearchCriteria;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq.SearchProviders
{
    public class UmbracoSearch : ISearcher
    {
        protected string IndexName { get; set; }

        public static Dictionary<string, ISearchCriteria> searchQueryCache { get; set; }

        static UmbracoSearch()
        {
            searchQueryCache = new Dictionary<string, ISearchCriteria>();
        }

        public UmbracoSearch(string indexName)
        {
            IndexName = indexName;
        }

        public IEnumerable<SearchResult> Search(string query)
        {
            ISearchCriteria criteria = null;
            var searcher = ExamineManager.Instance.SearchProviderCollection[IndexName];

            if (searchQueryCache.ContainsKey(query))
                criteria = searchQueryCache[query];
            else
            {
                criteria = searcher.CreateSearchCriteria();
                criteria = criteria.RawQuery(query);
                searchQueryCache.Add(query, criteria);
            }

            return searcher.Search(criteria);
        }
    }
}
