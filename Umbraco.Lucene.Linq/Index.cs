using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Lucene.Linq
{
    public class Index<T> : QueryableBase<T>
    {
        public Index()
            : base(new DefaultQueryProvider(typeof(Index<>), QueryParser.CreateDefault(), new Executor()))
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
