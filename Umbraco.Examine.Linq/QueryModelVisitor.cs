using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System.Linq.Expressions;

namespace Umbraco.Examine.Linq
{
    public class QueryModelVisitor : QueryModelVisitorBase
    {
        public List<StringBuilder> queries = new List<StringBuilder>();
        public int take = -1;
        public int skip = 0;
        public string orderByField = "";
        public bool orderByAsc = true;

        public override void VisitWhereClause(Remotion.Linq.Clauses.WhereClause whereClause, QueryModel queryModel, int index)
        {
            var query = new StringBuilder();
            queries.Add(query);

            ExpressionTreeVisitor visitor = new ExpressionTreeVisitor(query);
            visitor.VisitExpression(whereClause.Predicate);

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitMainFromClause(Remotion.Linq.Clauses.MainFromClause fromClause, QueryModel queryModel)
        {
            queries = new List<StringBuilder>();

            NodeTypeAliasAttribute attribute = (NodeTypeAliasAttribute)fromClause.ItemType.GetCustomAttributes(typeof(NodeTypeAliasAttribute), false).FirstOrDefault();

            if (attribute != null)
            {
                queries.Add(new StringBuilder(string.Format("nodeTypeAlias:{0}", attribute.Name)));
            }
            
            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            foreach (var resultOperator in queryModel.ResultOperators)
            {
                if (resultOperator is TakeResultOperator)
                    take = (int)((ConstantExpression)((TakeResultOperator)resultOperator).Count).Value;
                else if (resultOperator is SkipResultOperator)
                    skip = (int)((ConstantExpression)((SkipResultOperator)resultOperator).Count).Value;
            }
            base.VisitQueryModel(queryModel);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            var expression = orderByClause.Orderings[0].Expression as MemberExpression;
            var attr = expression.Member.GetCustomAttributes(typeof(Umbraco.Examine.Linq.Attributes.FieldAttribute), false);

            string fieldName = "";

            if (attr.Length > 0)
                fieldName = ((FieldAttribute)attr[0]).Name;
            else
            {
                fieldName = orderByClause.Orderings[0].Expression.Type.Name;
            }

            orderByAsc = orderByClause.Orderings[0].OrderingDirection == OrderingDirection.Asc;
            orderByField = fieldName;
        }
    }
}
