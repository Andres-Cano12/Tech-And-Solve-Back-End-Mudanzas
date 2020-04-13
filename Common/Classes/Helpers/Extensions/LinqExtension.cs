using App.Common.Classes.Helpers.DynamicFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace App.Common.Classes.Extensions
{
    public static class LinqExtension
    {
        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> source,
            string propertyName, string orderType = "asc")
        {
            var expression = PropertyAccesor<T>.Get(propertyName);
            if (expression == null)
            {
                return source;
            }

            //Ordenar ascendentemente por defecto
            var type = nameof(Queryable.OrderBy);

            //Ordenar descendentemente si es otro valor
            if (!string.IsNullOrEmpty(orderType) && orderType == "desc")
            {
                type = nameof(Queryable.OrderByDescending);
            }
            
            MethodCallExpression resultExpression = Expression.Call(
                typeof(Queryable),
                type,
                new Type[] { typeof(T), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));

            return source.Provider.CreateQuery<T>(resultExpression);


        }

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> source, string propertyName,
            object propertyValue)
        {
            var mba = PropertyAccesor<T>.Get(propertyName);
            if (mba == null)
            {
                return source;
            }
            object value;
            try
            {
                value = Convert.ChangeType(propertyValue, mba.ReturnType);
            }
            catch (Exception ex)
            {
                return source;
            }

            var eqe = Expression.Equal(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));

            var expression = Expression.Lambda(eqe, mba.Parameters[0]);
            

            MethodCallExpression resultExpression = Expression.Call(
               null,
               GetMethodInfo(Queryable.Where, source, (Expression<Func<T, bool>>)null),
               new Expression[] { source.Expression, Expression.Quote(expression) });

            return source.Provider.CreateQuery<T>(resultExpression);
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(
            Func<T1, T2, T3> f, T1 unused1, T2 unused2)
        {
            return f.Method;
        }

    }
}
