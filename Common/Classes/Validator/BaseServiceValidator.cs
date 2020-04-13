using App.Common.Classes.Exceptions;
using App.Common.Classes.Extensions;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Common.Classes.Validator
{
    public abstract class BaseServiceValidator<T, TValidator> : IServiceValidator<T> where T : class
	{
        protected IStringLocalizer<TValidator> _localizer;

        protected LocalValidator<T> PreValidateValidator { get; set; }

        protected LocalValidator<T> PreInsertValidator { get; set; }
        protected LocalValidator<T> PostInsertValidator { get; set; }

        protected LocalValidator<T> PreUpdateValidator { get; set; }
        protected LocalValidator<T> PostUpdateValidator { get; set; }

        protected LocalValidator<T> PreDeleteValidator { get; set; }
        protected LocalValidator<T> PostDeleteValidator { get; set; }
        
        public BaseServiceValidator(IStringLocalizer<TValidator> localizer)
		{
            _localizer = localizer;

            PreInsertValidator = new LocalValidator<T>();
            PostInsertValidator = new LocalValidator<T>();

            PreUpdateValidator = new LocalValidator<T>();
            PostUpdateValidator = new LocalValidator<T>();

            PreDeleteValidator = new LocalValidator<T>();
            PostDeleteValidator = new LocalValidator<T>();

            LoadPreInsertRules();
            LoadPostInsertRules();

            LoadPreUpdateRules();
            LoadPostUpdateRules();

            LoadPreDeleteRules();
            LoadPostDeleteRules();
        }

		public abstract void LoadPreInsertRules();
		public abstract void LoadPostInsertRules();

		public abstract void LoadPreUpdateRules();
		public abstract void LoadPostUpdateRules();

		public abstract void LoadPreDeleteRules();
		public abstract void LoadPostDeleteRules();
        
		private void ValidateResult(ValidationResult result)
		{
			if (!result.IsValid)
			{
				List<string> errors = new List<string>();
				foreach (ValidationFailure valResult in result.Errors)
				{
					errors.Add(valResult.ErrorMessage);
				}
				throw new ValidationServiceException(errors);
			}
		}
        
        public void PreValidate(T entity)
        {
            ValidateResult(PreInsertValidator.Validate(entity));
        }

        public void PreInsert(T entity)
		{
			ValidateResult(PreInsertValidator.Validate(entity));
		}

		public void PostInsert(T entity)
		{
			ValidateResult(PostInsertValidator.Validate(entity));
		}
        
		public void PreUpdate(T entity)
		{
			ValidateResult(PreUpdateValidator.Validate(entity));
		}

		public void PostUpdate(T entity)
		{
			ValidateResult(PostUpdateValidator.Validate(entity));
		}

		public void PreDelete(T entity)
		{
			ValidateResult(PreDeleteValidator.Validate(entity));
		}

		public void PostDelete(T entity)
		{
			ValidateResult(PostDeleteValidator.Validate(entity));
		}

		public virtual bool BeAValidDate(DateTime? date)
		{
            return date.HasValue ? !(date.Equals(default(DateTime))) : false;
        }

        public virtual bool ValidNumberLength(object item, int minLength, int maxLength)
        {
            if (item != null && minLength >= 0 && maxLength >= 0)
            {
                string value = item.ToSafeString();
                return value.Length >= minLength && value.Length <= maxLength ? true : false;
            }

            return false;
        }

        public virtual bool ValidDecimalLength(object item, int integerLength, int decimalLength)
        {
            if (item != null && integerLength >= 0 && decimalLength >= 0)
            {
                string value = item.ToString();
                var valueSplit = value.Split('.');

                // Parte entera
                bool result = valueSplit[0].Length <= integerLength;

                // Parte Decimal
                result = valueSplit.Count() > 1 ? valueSplit[1].Length <= decimalLength : result;

                return result;
            }

            return false;
        }

        public virtual bool ValidRegex(string expression, object item)
        {
            string value = item.ToSafeString();

            if (!string.IsNullOrEmpty(expression) && !string.IsNullOrEmpty(value))
            {
                Regex regex = new Regex(expression);
                Match match = regex.Match(value);
                return match.Success;
            }

            return false;
        }
    }
}