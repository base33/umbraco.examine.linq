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
        public double fuzzy = 0;

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

            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    currentPart.Append("");
                    break;

                case ExpressionType.NotEqual:
                    currentPart.Insert(0, " NOT ");
                    break;

                case ExpressionType.GreaterThan:
                    currentPart.Append("gt"); //constant expression uses this to decide how to output the value
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    currentPart.Append("ge"); //constant expression uses this to decide how to output the value
                    break;

                case ExpressionType.LessThan:
                    currentPart.Append("lt"); //constant expression uses this to decide how to output the value
                    break;

                case ExpressionType.LessThanOrEqual:
                    currentPart.Append("le"); //constant expression uses this to decide how to output the value
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

            if(expression.Value is string)
            {
                value = expression.Value.ToString();
                if (value.Contains(' '))
                    value = string.Format("\"{0}\"", value);
            }
            else if (expression.Value is string[])
                value = string.Join(" ", (string[])expression.Value);
            else if (expression.Value is IEnumerable<string>)
                value = string.Join(" ", (IEnumerable<string>)expression.Value);
            else if (expression.Value is DateTime)
            {
                var formattedDateTime = ((DateTime)expression.Value).ToString("o");
                string operation = currentPart.ToString().Substring(currentPart.Length - 2);
                if(!handleRangeOperation(formattedDateTime, operation, ref value))
                {
                    value = formattedDateTime;
                }
            }
            else if(expression.Value is int)
            {
                var formattedInt = ((int)expression.Value).ToString();
                string operation = currentPart.ToString().Substring(currentPart.Length - 2);
                if (!handleRangeOperation(formattedInt, operation, ref value))
                {
                    value = formattedInt;
                }
            }
            else if (expression.Value is bool || expression.Value is Boolean)
                value = ((bool)expression.Value) ? "1" : "0";
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
                    VisitConstantExpression((ConstantExpression)expression.Arguments[0]);
                    currentPart.Append(getFuzzyString());
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
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    currentPart.AppendFormat("^{0}", expression.Arguments[1]);
                    break;
                case "Fuzzy":
                    bracketsEnabled = false;
                    fuzzy = (double)((ConstantExpression)expression.Arguments[1]).Value;
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    fuzzy = 0;
                    bracketsEnabled = true;
                    break;
            }

            query.Append(currentPart);
            addEndBracket();
            currentParts.Pop();

            return expression;
        }

        protected bool handleRangeOperation(string formattedValue, string operation, ref string value)
        {
            switch (operation)
            {
                case "lt":
                    value = string.Format("* TO {0}]", formattedValue);
                    currentPart.Length = currentPart.Length - 2; //clear the last 2 characters
                    break;
                case "gt":
                    value = string.Format("[{0} TO *]", formattedValue);
                    currentPart.Length = currentPart.Length - 2; //clear the last 2 characters
                    break;
                case "le":
                    value = string.Format("[* TO {0}]", formattedValue);
                    currentPart.Length = currentPart.Length - 2; //clear the last 2 characters
                    break;
                case "ge":
                    value = string.Format("[{0} TO *]", formattedValue);
                    currentPart.Length = currentPart.Length - 2; //clear the last 2 characters
                    break;
                default: //is equals
                    return false;
            }
            return true;
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

        protected string getFuzzyString()
        {
            return fuzzy > 0 ? "~" + fuzzy.ToString() : "";
        }
    }
}
