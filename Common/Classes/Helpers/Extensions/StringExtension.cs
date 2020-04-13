using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace App.Common.Classes.Extensions
{
    public static class StringExtension
    {
        public static string ToSafeString(this object value)
        {            
            if (null != value)
            {
                var type = value.GetType();
                if (type == typeof(decimal))
                {
                    decimal data = (decimal)value;

                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";
                    
                    return data.ToString(nfi);
                }
            }
            else
            {
                return string.Empty;
            }

            
            return value != null ? value.ToString() : string.Empty;
        }

        public static string ToSafeLower(this object value)
        {
            return value != null ? value.ToString().ToLower() : string.Empty;
        }

        public static string ToBase64(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(plainTextBytes);
            }
            return text;

        }

        public static string ToTextFromBase64(this string base64Text)
        {
            if (!string.IsNullOrEmpty(base64Text))
            {
                var encodedTextBytes = Convert.FromBase64String(base64Text);

                return Encoding.UTF8.GetString(encodedTextBytes);
            }

            return base64Text;
        }

        public static string RemoveSpaces(this string text)
        {
            return text?.Replace(" ", string.Empty);
        }

        public static string ReplaceAccents(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("á", "a");
                text = text.Replace("é", "e");
                text = text.Replace("í", "i");
                text = text.Replace("ó", "o");
                text = text.Replace("ú", "u");
                text = text.Replace("ñ", "n");
                text = text.Replace("ü", "u");

                text = text.Replace("Á", "A");
                text = text.Replace("É", "E");
                text = text.Replace("Í", "I");
                text = text.Replace("Ó", "O");
                text = text.Replace("Ú", "U");
                text = text.Replace("Ñ", "N");
                text = text.Replace("Ü", "U");
            }

            return text;

        }

        public static string MessageToUlHtmlString(this List<string> list)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ul>");

            foreach (var item in list)
            {
                sb.Append(string.Format("<li>{0}</li>", item));
            }

            sb.Append("</ul>");

            return (sb.ToString());
        }
    }
}
