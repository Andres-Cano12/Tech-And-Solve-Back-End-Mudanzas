using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace App.Common.Classes.Helpers.DynamicFilters
{
    public static class PropertyExpression
    {
        public static Expression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "c");
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return body;
        }

        public static Expression CreateExpression(ParameterExpression param,
            Type type, string propertyName)
        {
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return body;
        }


    }
}
