using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace App.Common.Classes.Helpers.DynamicFilters
{
    public static class PropertyAccesor<T> 
    {
        private static readonly IDictionary<string, LambdaExpression> _cache;

        static PropertyAccesor()
        {
            var storage = new Dictionary<string, LambdaExpression>();

            var type = typeof(T);
            var parameter = Expression.Parameter(type, "c");
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var lambdaExpression = Expression.Lambda(propertyAccess, parameter);

                storage[property.Name] = lambdaExpression;

                _cache = storage;
            }
        }

        public static LambdaExpression Get(string propertyName)
        {
            LambdaExpression result;
            return _cache.TryGetValue(propertyName, out result) ? result : null;

        }

    }
}
