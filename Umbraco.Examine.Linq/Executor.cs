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
    public class Executor : IQueryExecutor
    {
        protected ISearcher Searcher { get; set; }

        public Executor(ISearcher searcher)
        {
            Searcher = searcher;
        }

        // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var visitor = new QueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            var searchResults = Searcher.Search(visitor.query.ToString());
            return new SearchResultMapper<T>().Map(searchResults);
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
