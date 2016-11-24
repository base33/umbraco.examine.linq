using Examine;
using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Mapper;

namespace Umbraco.Examine.Linq
{
    public class Executor<T> : IQueryExecutor
    {
        protected ISearcher Searcher { get; set; }
        protected IMapper<T> Mapper { get; set; }

        public Executor(ISearcher searcher, IMapper<T> mapper)
        {
            Searcher = searcher;
            Mapper = mapper ?? new SearchResultMapper<T>();
        }

        // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var visitor = new QueryModelVisitor();
            visitor.VisitQueryModel(queryModel);

            string query = string.Join(" AND ", visitor.andQueries);
            if (visitor.notQueries.Any())
            {
                query += " NOT " + string.Join(" NOT ", visitor.notQueries); 
            }

            var searchResults = Searcher.Search(query);

            if (visitor.skip != -1)
                searchResults = searchResults.Skip(visitor.skip);

            if (visitor.take != -1)
            {
                searchResults = searchResults.Take(visitor.take);
            }

            return (IEnumerable<T>)Mapper.Map(searchResults);
        }

        // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel).Single();
        }

        // Executes a query with a collection result.
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();
        }
    }
}
