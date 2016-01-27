using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Models;
using Umbraco.Examine.Linq.SearchProviders;

namespace Umbraco.Examine.Linq
{
    public class Index<T> : QueryableBase<T>
    {
        public Index(IMapper<T> mapper)
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor<T>(new LuceneSearch("ExternalSearcher"), mapper)))
        {

        }

        public Index(string indexName = "ExternalSearcher", IMapper<T> mapper = null)
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor<T>(new LuceneSearch(indexName), mapper)))
        {

        }

        public Index(ISearcher searcher, IMapper<T> mapper = null)
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor<T>(searcher, mapper)))
        {

        }

        public Index(IQueryParser queryParser, IQueryExecutor executor)
            : base(new DefaultQueryProvider(typeof(Index<>), queryParser, executor))
        {
        }

        public Index(IQueryProvider provider, Expression expression)
            : base(provider, expression)
        {
        }
    }
}
