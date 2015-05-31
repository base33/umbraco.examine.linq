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
        public Index(string indexName = "ExternalSearcher")
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor(new UmbracoSearch(indexName))))
        {

        }

        public Index(ISearcher searcher)
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor(searcher)))
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
