using App.Common.Classes.DTO.Request;
using App.Common.Classes.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.Text;


namespace App.Common.Classes.Helpers.DynamicFilters
{
    public class ExpressionBuilder
    {
        // Metodos predefinidos de consulta
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> GetExpression<T>(List<FilterParams> filters)
        {

            if (filters == null)
            {
                return null;
            }

            if (filters.Count == 0)
            {
                return null;
            }
                

            // Crea el parametro para el tipo de objeto (eje 'c' en la expresion (c => 'c')
            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            // Para almacenar el resultado de la expresion calculada
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]);
            }
                
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(param, filters[0], filters[1]);
            }
               
            else
            {
                // Se recorren los filtros para obteenr la expresion por cada uno
                while (filters.Count > 0)
                {
                    
                    var f1 = filters[0];
                    var f2 = filters[1];

                    
                    if (exp == null)
                    {
                        exp = GetExpression<T>(param, filters[0], filters[1]); 
                    }
                        
                    else
                    {
                        // Se va agregando a la expresion 
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));
                    }
                        

                    filters.Remove(f1);
                    filters.Remove(f2);

                    // Los Impares se manejar por separado
                    if (filters.Count == 1)
                    {
                        
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));

                       
                        filters.RemoveAt(0);
                    }
                }
            }

            return exp != null ? Expression.Lambda<Func<T, bool>>(exp, param) : null;
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterParams filter)
        {
            
            object value;
            Expression member = null;
            try
            {
                var type = typeof(T);
                // El miembro, propiedad que se quiere evaluar (x => x.FirstName)
                member = PropertyExpression.CreateExpression(param, type, filter.PropertyName);

                var targetType = ObjectExtension.IsNullableType(member.Type)
                    ? Nullable.GetUnderlyingType(member.Type) : member.Type;

                value = Convert.ChangeType(filter.Value, targetType);
                //si es string se pasa el valor a minuscula
                if (value.GetType() == typeof(string))
                {
                    value = value.ToSafeLower();
                }

                ConstantExpression constant = Expression.Constant(value);

                //Si la propiedad es nullable se convierte al mismo tipo del valor
                if (ObjectExtension.IsNullableType(member.Type))
                {
                    member = Expression.Convert(member, constant.Type);
                }                

                // Aplicar las expresiones de operacion para la consulta
                switch (filter.Operator)
                {
                    case "EqualsString":
                        //si el valor es string se pasa a minuscula la expresion
                        if (value.GetType() == typeof(string))
                        {
                            var expreLower = Expression.Call(member, "ToLower", null);
                            return Expression.Equal(expreLower, constant);
                        }
                        return Expression.Equal(member, constant);

                    case "Equals":
                        //si el valor es string se pasa a minuscula la expresion
                        if (value.GetType() == typeof(string))
                        {
                            var expreLower = Expression.Call(member, "ToLower", null);
                            return Expression.Equal(expreLower, constant);
                        }
                        return Expression.Equal(member, constant);

                    case "Contains":
                        var expresionLower = Expression.Call(member, "ToLower", null);
                        return Expression.Call(expresionLower, containsMethod,  constant);

                    case "GreaterThan":
                        return Expression.GreaterThan(member, constant);

                    case "GreaterThanOrEqual":
                        return Expression.GreaterThanOrEqual(member, constant);

                    case "LessThan":
                        return Expression.LessThan(member, constant);

                    case "LessThanOrEqualTo":
                        return Expression.LessThanOrEqual(member, constant);

                    case "StartsWith":
                        return Expression.Call(member, startsWithMethod, constant);

                    case "EndsWith":
                        return Expression.Call(member, endsWithMethod, constant);

                    default:
                        return DefaultExpression<T>(param);
                }
            }
            catch (Exception ex)
            {
     
                return DefaultExpression<T>(param);
            }

        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, FilterParams filter1, FilterParams filter2)
        {
            Expression result1 = GetExpression<T>(param, filter1);
            Expression result2 = GetExpression<T>(param, filter2);


            return result1 != null && result2 != null ? Expression.AndAlso(result1, result2): null;
        }

        //Cuando una propiedad no existe o se genera una excepcion se devuelve esta expresion
        private static Expression DefaultExpression<T>(ParameterExpression param)
        {
            //se toma el Id que es comun para todos los modelos
            var mba = PropertyAccesor<T>.Get("Id");
            object value = Convert.ChangeType(1, mba.ReturnType);
           
            MemberExpression member = Expression.Property(param, "Id");
            ConstantExpression constant = Expression.Constant(value);

            return Expression.GreaterThanOrEqual(member, constant);
        }


    }
}
