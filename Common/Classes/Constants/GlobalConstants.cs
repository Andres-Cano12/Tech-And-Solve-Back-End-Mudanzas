using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Constants
{
    public class GlobalConstants
    {
        public const string WEB_CLIENT_EXCEPTION = "No podemos procesar la petición, inténtelo más tarde.";
        public const string WEB_CLIENT_UNAUTHORIZED = "Token no valido para hacer la petición";
        public const string WEB_CLIENT_FORBIDDEN = "Token no permitido para hacer la petición";

        #region Messages

        public const string ERROR_BASE_MODEL_ONLY_ONE_KEY = "El objeto tiene mas de 1 llave primaria. Lo cual no permite la automatización de BaseModel en el Update";

        public const string ERROR_BASE_MODEL_PRIMARY_KEY = "El objeto no tiene llave primaria. Lo cual no permite la automatización de BaseModel en el Update";

        public const string OPERATION_PROCESS_OK = "Operación Elaborada Con Éxito";

        #endregion

        #region Routes

        public const string ROUTE_GET_ALL = "api/{0}/getAll";

        public const string ROUTE_FIND_BY_ID = "api/{0}/details/{1}";

        public const string ROUTE_CREATE = "api/{0}/create";

        public const string ROUTE_EDIT = "api/{0}/edit";

        public const string ROUTE_DELETE = "api/{0}/delete";

        #endregion

        #region Filter

        public const string FORMAT_ANY = "any";

        public const string FORMAT_STRING = "string";

        public const string FORMAT_NUMERIC = "numeric";

        public const string FORMAT_DATE = "date";

        public const string FORMAT_BOOLEAN = "boolean";

        public const string CONDITION_CONTAINS = "Contiene";

        public const string CONDITION_GREATER_THAN = "Mayor a";

        public const string CONDITION_GREATER_THAN_OR_EQUAL = "Mayor o igual a";

        public const string CONDITION_LESS_THAN = "Menor a";

        public const string CONDITION_LESS_THAN_OR_EQUAL_TO = "Menor o igual a";

        public const string CONDITION_STARTS_WITH = "Inicia con";

        public const string CONDITION_ENDS_WITH = "Termia con";

        public const string CONDITION_EQUALS = "Igual a";

        public const string CONDITION_NOT_EQUAL = "Diferente a";

        public const string DTO_NAMESPACE = "App.Common.Classes.DTO.";

        #endregion
    }
}
