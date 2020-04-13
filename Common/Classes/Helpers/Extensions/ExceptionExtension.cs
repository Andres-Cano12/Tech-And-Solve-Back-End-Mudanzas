using App.Common.Classes.Exceptions;
using System;
using System.Text;

namespace App.Common.Classes.Extensions
{
    public static class ExceptionExtension
	{

		public static string ToUlHtmlString(this Exception ex)
		{
			Type type = ex.GetType();

			StringBuilder sb = new StringBuilder();
			sb.Append("<ul>");

			switch (type.FullName)
			{
				case "App.Common.Classes.Exceptions.ValidationServiceException":

                    ValidationServiceException valService = ex as ValidationServiceException;

                    foreach (var item in valService.ErrorMessages)
					{
						sb.Append(string.Format("<li>{0}</li>", item));
					}
					break;
                case "System.ApplicationException":
                    sb.Append($"<li>{ex.Message}</li>");
                    break;
				default:
					sb.Append("<li>Ha ocurrido un error, por favor intentarlo más tarde</li>");
					break;
			}

			sb.Append("</ul>");

			return (sb.ToString());

		}


	}
}
