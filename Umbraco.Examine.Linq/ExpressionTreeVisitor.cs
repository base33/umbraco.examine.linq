using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq
{
    public class ExpressionTreeVisitor : Remotion.Linq.Parsing.ExpressionTreeVisitor
    {
        public StringBuilder query;
        public Stack<StringBuilder> currentParts;
        public bool bracketsEnabled = true;
        public double fuzzy = 0;
        public bool inverseMode = false; //this means that the current expression is wanting the opposite - ei !r.Name.Contains("test"), so  +nodeName:test should be -nodeName:test
        public int proximity = 0;

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

            query.Append("(");

            VisitExpression(expression.Left);

            //bool localInverseActive = false;

            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    currentPart.Append("eq");
                    break;
                case ExpressionType.NotEqual:
                    inverseMode = true;
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

                case ExpressionType.Not:
                    query.Append("");
                    break;
            }
            bracketsEnabled = true;
            VisitExpression(expression.Right);
            inverseMode = false;
            //specific case where exact matches are required, we will insert the + or - at the start and a proximity of 0
            //may need to relocate this logic
            if(expression.Left is MemberExpression && expression.Right is ConstantExpression && ((ConstantExpression)expression.Right).Value is string)
            {
                if ((expression.NodeType == ExpressionType.NotEqual && !inverseMode) || (expression.NodeType == ExpressionType.Equal && inverseMode))
                    query.Insert(query.Length - 2, " NOT ");
                else
                    currentPart.Insert(0, "+");

                //currentPart.Insert(0, (expression.NodeType == ExpressionType.Equal) && !inverseMode || (
                //    (expression.NodeType == ExpressionType.NotEqual) & inverseMode & localInverseActive) ? "+" : "-");
            }

            //if (localInverseActive)
            //    inverseMode = !inverseMode;

            query.Append(currentPart);
            query.Append(")");
            currentParts.Pop();

            return expression;
        }

        protected override Expression VisitUnaryExpression(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Not)
                query.Append(" NOT ");

            VisitExpression(expression.Operand);

            return expression;
        } 

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            FieldAttribute attribute = GetReferenceSourceAttributeOrSelf((expression));

            if (attribute != null)
                currentPart.AppendFormat("{0}:", attribute.Name);
            else
                currentPart.AppendFormat("{0}:", expression.Member.Name);

            return expression;
        }

        protected FieldAttribute GetReferenceSourceAttributeOrSelf(MemberExpression memberExp)
        {
            //if(memberInfo is GetReferenceSourceAttributeOrSelf)
            if (!(memberExp.Expression is QuerySourceReferenceExpression))
            {
                MemberInfo memberIno = null;
                MemberExpression exp = memberExp;
                do
                {
                    exp = (MemberExpression)exp.Expression;
                } while (!(exp.Expression is QuerySourceReferenceExpression));
                return (FieldAttribute)exp.Member.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault();
            }

            return (FieldAttribute)memberExp.Member.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault();
        }

        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            string value = "";
			string operation = "";

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
                var formattedDateTime = ((DateTime)expression.Value).ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);//((DateTime)expression.Value).ToString("o");
                operation = currentPart.ToString().Substring(currentPart.Length - 2);
                if (operation == "eq")
                {
                    value = formattedDateTime;
                }
                else if (operation == "ne")
                {
                    string fieldName = currentPart.ToString().Substring(0, currentPart.Length - 2);
                    //we will do a < and then >
                    operation = "gt";
                    handleRangeOperation(formattedDateTime, "\\-99999999999999999", "99999999999999999", operation, ref value);

                    //we want to now create the greater than expression
                    value += " OR " + fieldName;
                    operation = "lt";
                    handleRangeOperation(formattedDateTime, "\\-99999999999999999", "99999999999999999", operation, ref value);
                }
                else
                {
                    handleRangeOperation(formattedDateTime, "\\-99999999999999999", "99999999999999999", operation, ref value);
                }
            }
            else if(expression.Value is int || expression.Value is double)
            {
                var formattedInt = expression.Value is double ? (Convert.ToInt64((double)expression.Value)).ToString() : ((int)expression.Value).ToString();
                operation = currentPart.ToString().Substring(currentPart.Length - 2);
                if(operation == "eq")
                {
                    value = formattedInt;
                }
                else if (operation == "ne")
                {
                    string fieldName = currentPart.ToString().Substring(0, currentPart.Length - 2);
                    //we will do a < and then >
                    operation = "gt";
                    handleRangeOperation(formattedInt, "\\-99999999999999999", "99999999999999999", operation, ref value);

                    //we want to now create the greater than expression
                    value += " OR " + fieldName;
                    operation = "lt";
                    handleRangeOperation(formattedInt, "\\-99999999999999999", "99999999999999999", operation, ref value);
                }
                else
                {
                    handleRangeOperation(formattedInt, "\\-99999999999999999", "99999999999999999", operation, ref value);
                }
            }
            else if (expression.Value is bool || expression.Value is Boolean)
                value = ((bool)expression.Value) ? "1" : "0";
            else
                value = expression.Value.ToString();

			if (!string.IsNullOrEmpty(operation))
				currentPart.Length = currentPart.Length - 2; //clear the last 2 characters

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
                    bracketsEnabled = false;
                    currentPart.Append(inverseMode ? "-" : "+");
                    VisitExpression(expression.Object);
                    if(expression.Arguments.Count == 3)
                    {
                        VisitExpression(expression.Arguments[1]);
                        currentPart.Append("~" + fuzzy.ToString());
                    }
                    else
                        VisitExpression(expression.Arguments[0]);
                    currentPart.Append(getFuzzyString());
                    bracketsEnabled = true;
                    break;
                case "ContainsAny":
                    //in this instance, the first argument is an expression to the value, 
                    //so we will disable brackets and visit the expression
                    bracketsEnabled = false;
                    string[] optionalValuesToTest = (string[])((ConstantExpression) expression.Arguments[1]).Value;
                    for (var i = 0; i < optionalValuesToTest.Length; i++)
                    {
                        if (inverseMode)
                            currentPart.Append("-");
                        VisitExpression(expression.Arguments[0]);
                        VisitExpression(expression.Object);
                        currentPart.Append(optionalValuesToTest[i] + (fuzzy > 0 ? "~" + fuzzy.ToString() : "")); //string array
                        if (i < optionalValuesToTest.Length - 1)
                            currentPart.Append(" "); //maybe use " OR "
                    }
                    bracketsEnabled = true;
                    break;
                case "ContainsAll":
                    //in this instance, the first argument is an expression to the value, 
                    //so we will disable brackets and visit the expression
                    bracketsEnabled = false;
                    string[] requiredValuesToTest = (string[])((ConstantExpression)expression.Arguments[1]).Value;
                    for (var i = 0; i < requiredValuesToTest.Length; i++)
                    {
                        currentPart.Append(inverseMode ? "-" : "+");
                        VisitExpression(expression.Arguments[0]);
                        VisitExpression(expression.Object);
                        currentPart.Append(requiredValuesToTest[i] + (fuzzy > 0 ? "~" + fuzzy.ToString() : "")); //string array
                        if (i < requiredValuesToTest.Length - 1)
                            currentPart.Append(" "); //maybe use " OR "
                    }
                    bracketsEnabled = true;
                    break;
                case "IsAny":
                    //in this instance, the first argument is an expression to the value, 
                    //so we will disable brackets and visit the expression
                    bracketsEnabled = false;
                    int[] intValuesToTest = (int[])((ConstantExpression)expression.Arguments[1]).Value;
                    for (var i = 0; i < intValuesToTest.Length; i++)
                    {
                        if (inverseMode)
                            currentPart.Append("-");
                        VisitExpression(expression.Arguments[0]);
                        VisitExpression(expression.Object);
                        currentPart.Append(intValuesToTest[i].ToString()); //string array
                        if (i < intValuesToTest.Length - 1)
                            currentPart.Append(" "); //maybe use " OR "
                    }
                    bracketsEnabled = true;
                    break;
                case "Boost":
                    bracketsEnabled = false;
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    addEndBracket();
                    currentPart.AppendFormat("^{0}", expression.Arguments[1]);
                    bracketsEnabled = false;
                    break;
                case "Fuzzy":
                    bracketsEnabled = false;
                    fuzzy = (double)((ConstantExpression)expression.Arguments[1]).Value;
                    if(expression.Arguments.Any())
                        VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Object);
                    fuzzy = 0;
                    bracketsEnabled = false;
                    break;
				case "Equals":
					bracketsEnabled = false;
					VisitExpression(expression.Object);
					VisitExpression(expression.Arguments[0]);
					bracketsEnabled = true;
					break;
            }

            query.Append(currentPart);
            addEndBracket();
            currentParts.Pop();

            return expression;
        }

        protected bool handleRangeOperation(string formattedValue, string defaultMinValue, string defaultMaxValue, string operation, ref string value)
        {
            switch (operation)
            {
                case "le":
                    value += string.Format("[{0} TO {1}]", defaultMinValue, formattedValue);
                    break;
                case "lt":
                    formattedValue = (Int64.Parse(formattedValue) - 1).ToString();//-1 on the formattedValue to do less than
                    value += string.Format("[{0} TO {1}]", defaultMinValue, formattedValue);
                    break;
                case "ge":
                    value += string.Format("[{0} TO {1}]", formattedValue, defaultMaxValue);
                    break;
                case "gt":
                    formattedValue = (Int64.Parse(formattedValue) + 1).ToString();//-1 on the formattedValue to do less than
                    value += string.Format("[{0} TO {1}]", formattedValue, defaultMaxValue);
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
