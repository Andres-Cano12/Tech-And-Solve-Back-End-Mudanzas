using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Exceptions
{
    public class ValidationServiceException: System.Exception
    {
        public List<string> ErrorMessages { get; set; }

        public override string Message
        {
            get
            {
                return this.ErrorMessages.Count > 0 ? this.ErrorMessages[0] : string.Empty;
            }
        }

        public ValidationServiceException()
        {
            this.ErrorMessages = new List<string>();
        }

        public ValidationServiceException(List<string> messages)
        {
            this.ErrorMessages = messages;
        }

        public ValidationServiceException(string message, System.Exception inner)
          : base(message, inner)
        {
        }
    }
}
