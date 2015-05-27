using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Lucene.Linq.Attributes;

namespace Umbraco.Lucene.Linq
{
    public class QueryModelVisitor : QueryModelVisitorBase
    {
        public StringBuilder query = new StringBuilder();

        public override void VisitWhereClause(Remotion.Linq.Clauses.WhereClause whereClause, QueryModel queryModel, int index)
        {
            ExpressionTreeVisitor visitor = new ExpressionTreeVisitor(query);
            visitor.VisitExpression(whereClause.Predicate);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitMainFromClause(Remotion.Linq.Clauses.MainFromClause fromClause, QueryModel queryModel)
        {
            RecordTypeAttribute attribute = (RecordTypeAttribute)fromClause.ItemType.GetCustomAttributes(typeof(RecordTypeAttribute), false).FirstOrDefault();

            if (attribute != null)
                query.AppendFormat("nodeTypeAlias:{0} AND ",  attribute.Name);

            base.VisitMainFromClause(fromClause, queryModel);
        }
    }
}
