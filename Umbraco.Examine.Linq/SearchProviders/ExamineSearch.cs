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
    public class ExamineSearch : ISearcher
    {
        protected string IndexName { get; set; }

        public static Dictionary<string, ISearchCriteria> searchQueryCache { get; set; }

        static ExamineSearch()
        {
            searchQueryCache = new Dictionary<string, ISearchCriteria>();
        }

        public ExamineSearch(string indexName)
        {
            IndexName = indexName;
        }

        public IEnumerable<SearchResult> Search(string query, int skip, int take, string orderByField, bool orderByAsc)
        {
            ISearchCriteria criteria = null;
            var searcher = ExamineManager.Instance.SearchProviderCollection[IndexName];
            

            if (searchQueryCache.ContainsKey(query))
                criteria = searchQueryCache[query];
            else
            {
                criteria = searcher.CreateSearchCriteria();
                criteria = criteria.RawQuery(query);

                if (orderByField != "")
                    criteria = orderByAsc ? criteria.OrderBy(new[] { orderByField }).Compile() : criteria.OrderByDescending(new[] { orderByField }).Compile();

                searchQueryCache.Add(query, criteria);
            }

            var results = searcher.Search(criteria).Skip(skip);

            if (take > -1)
                results = results.Take(take);

            return results.ToList();
        }
    }
}
