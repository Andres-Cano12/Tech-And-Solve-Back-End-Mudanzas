using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Validator.FluentValidator
{
    public class NumericValidator: RegularExpressionValidator
    {
        public NumericValidator() : base("^[0-9]*$") { }
    }
}
