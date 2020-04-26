using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class QueryableUtil
    {
        public static bool IncludeInactive(IDictionary<string, string> queryParams)
        {
            return queryParams != null
                && queryParams.ContainsKey("IncludeInactive")
                && queryParams["IncludeInactive"].Equals("true");
        }
 /*        public static void IgnoreQueryFilters(IQueryable<object> result, IDictionary<string, string> queryParams)
        {
            if (queryParams.ContainsKey("IncludeInactive") && queryParams["IncludeInactive"].Equals("true"))
            {
                result = result
                .IgnoreQueryFilters();
            }
        } */

/*         public static IQueryable IgnoreQueryFilters<T>(IQueryable<object> result, IDictionary<string, string> queryParams)
        {
            if (queryParams != null &&
                    queryParams.ContainsKey("IncludeInactive") &&
                    queryParams["IncludeInactive"].Equals("true"))
            {
                return result
                    .IgnoreQueryFilters();
            }

            return result;
        } */

 /*        public static Expression MakeWhere(IQueryable<object> result, Type entityType, ParameterExpression parameterExpression, IDictionary<string, string> queryParams)
        {
            Expression where = null;

            foreach (KeyValuePair<string, string> kvp in queryParams)
            {
                if (kvp.Key != "sort" && kvp.Key != "IncludeInactive")
                {
                    if (where == null)
                    {
                        where = MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value);
                    }
                    else
                    {
                        where = Expression.And(where, MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value));
                    }
                }
            }

            return where;
        } */
        public static Expression MakeWhereClause(Type entityType, ParameterExpression parameterExpression, IDictionary<string, string> queryParams)
        {
            Expression where = null;

            foreach (KeyValuePair<string, string> kvp in queryParams)
            {
                if (kvp.Key != "sort" && kvp.Key != "IncludeInactive")
                {
                    if (where == null)
                    {
                        where = MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value);
                    }
                    else
                    {
                        where = Expression.And(where, MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value));
                    }
                }
            }

            return where;
        }

        private static Expression MakePropertyClause(Type entityType, ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            PropertyInfo propertyInfo = entityType.GetProperty(memberName);

            if (propertyInfo == null)
            {
                throw new ArgumentException(String.Format("O campo {0} não existe.", memberName));
            }

            Expression expression = null;

            switch (propertyInfo.PropertyType.Name)
            {
                case "Int32":
                    expression = MakePropertyIntClause(parameterExpression, memberName, memberValue);
                    break;

                // String
                default:
                    expression = MakePropertyStringClause(parameterExpression, memberName, memberValue);
                    break;
            }
            return expression;
        }

        private static Expression MakePropertyStringClause(ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            Expression fieldProperty = Expression.Property(parameterExpression, memberName);
            Expression fieldClause = Expression.Equal(fieldProperty, Expression.Constant(memberValue));

            return Expression.Equal(fieldProperty, Expression.Constant(memberValue));
        }

        private static Expression MakePropertyIntClause(ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            Expression fieldProperty = Expression.Property(parameterExpression, memberName);
            return Expression.Equal(fieldProperty, Expression.Constant(Convert.ToInt32(memberValue), typeof(int)));
        }
    }
}