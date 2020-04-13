using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace App.Common.Classes.Extensions
{
    public static class ObjectExtension
    {
        public static object GetPropertyValue(this object entity, string propertyName)
        {
            var entityProperty = entity.GetType().GetProperties()
                .FirstOrDefault(c => c.Name == propertyName);

            if (entityProperty != null)
            {
                return entityProperty.GetValue(entity, null);
            }

            return null;

        }

        public static object GetPrimaryKeyValue(this object entity)
        {
            // Obtenemos el nombre de la clase
            string className = entity.GetType().Name;

            // Limpiamos el nombre en caso de que sea un DTO para que extraer el nombre 
            // del modelo
            className = className.TrimEnd('D', 'T', 'O');

            // Definimos el nombre de la llave primaria el cual es por estandar:
            //      NombreModelo + Id = NombreModeloId
            //      Ej. ProductStockId
            className = className + "Id";

            // Retornamos el valor de la propiedad
            return entity.GetPropertyValue(className);
        }


        public static object SetPropertyValue(this object entity, string propertyName, object value)
        {
            var entityProperty = entity.GetType().GetProperties()
                .FirstOrDefault(c => c.Name == propertyName);

            if (entityProperty != null)
            {
                var propertyType = entityProperty.PropertyType;

                //Para el manejo de propiedades nullables
                var targetType = IsNullableType(entityProperty.PropertyType) ? Nullable.GetUnderlyingType(entityProperty.PropertyType) : entityProperty.PropertyType;

                value = Convert.ChangeType(value, targetType);
                entityProperty.SetValue(entity, value, null);
            }

            return entity;

        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
