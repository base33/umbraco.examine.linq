using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Lucene.Linq.Attributes;

namespace Umbraco.Lucene.Linq
{
    public class ExpressionTreeVisitor : Remotion.Linq.Parsing.ExpressionTreeVisitor
    {
        public StringBuilder query;
        public Stack<StringBuilder> currentParts;
        public bool bracketsEnabled = true;

        public StringBuilder currentPart
        {
            get
            {
                return currentParts.First();
            }
        }

        public ExpressionTreeVisitor(StringBuilder query)
        {
            this.query = query;
            this.currentParts = new Stack<StringBuilder>();
        }

        protected override System.Linq.Expressions.Expression VisitBinaryExpression(System.Linq.Expressions.BinaryExpression expression)
        {
            this.currentParts.Push(new StringBuilder());

            addStartBracket();

            VisitExpression(expression.Left);

            // In production code, handle this via lookup tables.
            
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    currentPart.Append("");
                    break;

                case ExpressionType.NotEqual:
                    currentPart.Insert(0, " NOT ");
                    break;

                case ExpressionType.GreaterThan:
                    currentPart.Append(" gt ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    currentPart.Append(" ge ");
                    break;

                case ExpressionType.LessThan:
                    currentPart.Append(" lt ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    currentPart.Append(" le ");
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    query.Append(" AND ");
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    query.Append(" OR ");
                    break;
            }

            VisitExpression(expression.Right);

            query.Append(currentPart);
            addEndBracket();
            currentParts.Pop();

            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            FieldAttribute attribute = (FieldAttribute)expression.Member.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault();

            if (attribute != null)
                currentPart.AppendFormat("{0}:", attribute.Name);
            else
                currentPart.AppendFormat("{0}:", expression.Member.Name);

            return expression;
        }

        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            string value = "";

            if (expression.Value is string[])
                value = string.Join(" ", (string[])expression.Value);
            else if (expression.Value is IEnumerable<string>)
                value = string.Join(" ", (IEnumerable<string>)expression.Value);
            else if (expression.Value is DateTime)
                value = ((DateTime)expression.Value).ToString("o");
            else
                value = expression.Value.ToString();

            currentPart.Append(value);

            return expression;
        }

        protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
        {
            addStartBracket();
            currentParts.Push(new StringBuilder());

            switch(expression.Method.Name)
            {
                case "Contains":
                    currentPart.Append("+");
                    VisitExpression(expression.Object);
                    currentPart.AppendFormat("{0}", string.Join(" ", expression.Arguments));
                    break;
                case "ContainsAny":
                    //in this instance, the first argument is an expression to the value, 
                    //so we will disable brackets and visit the expression
                    bracketsEnabled = false;
                    currentPart.Append("+");
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    currentPart.AppendFormat("{0}", string.Join(" ", ((string[])((ConstantExpression) expression.Arguments[1]).Value))); //string array
                    bracketsEnabled = true;
                    break;
                case "StartsWith":
                    VisitExpression(expression.Object);
                    currentPart.Append(expression.Arguments);
                    break;
                case "Boost":
                    //in this instance, the first argument is an expression to the value, 
                    //so we will disable brackets and visit the expression
                    bracketsEnabled = false;
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    currentPart.AppendFormat("^{0}", expression.Arguments[1]);
                    bracketsEnabled = true;
                    break;
            }

            query.Append(currentPart);
            addEndBracket();
            currentParts.Pop();

            return expression;
        }

        public void addStartBracket()
        {
            if(bracketsEnabled)
                query.Append("(");
        }

        public void addEndBracket()
        {
            if (bracketsEnabled)
                query.Append(")");
        }
    }
}
