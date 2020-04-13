using App.Common.Classes.Validator.FluentValidator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Extensions
{
    public static class FluentValidationExtension
    {
        public static IRuleBuilderOptions<T, string> IsNumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NumericValidator());

        }
      
    }
}
