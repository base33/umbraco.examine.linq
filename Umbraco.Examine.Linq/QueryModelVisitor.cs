using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq
{
    public class QueryModelVisitor : QueryModelVisitorBase
    {
        public StringBuilder query = new StringBuilder();

        public override void VisitWhereClause(Remotion.Linq.Clauses.WhereClause whereClause, QueryModel queryModel, int index)
        {
            if (query.Length > 0)
                query.Append(" AND ");
            ExpressionTreeVisitor visitor = new ExpressionTreeVisitor(query);
            visitor.VisitExpression(whereClause.Predicate);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitMainFromClause(Remotion.Linq.Clauses.MainFromClause fromClause, QueryModel queryModel)
        {
            query.Clear();

            NodeTypeAliasAttribute attribute = (NodeTypeAliasAttribute)fromClause.ItemType.GetCustomAttributes(typeof(NodeTypeAliasAttribute), false).FirstOrDefault();

            if (attribute != null)
                query.AppendFormat("nodeTypeAlias:{0}",  attribute.Name);

            base.VisitMainFromClause(fromClause, queryModel);
        }
    }
}
