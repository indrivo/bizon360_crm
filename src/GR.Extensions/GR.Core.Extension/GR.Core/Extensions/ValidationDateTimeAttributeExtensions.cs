using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Core.Extensions
{
    public class ValidationDateTimeAttributeExtensions : ValidationAttribute
    {

        public ValidationDateTimeAttributeExtensions( string errorMessage, string compareOperator, object compareDate = null)
        {
            ErrorMessage = errorMessage;
            CompareDate = (DateTime?)compareDate;
            CompareOperator = compareOperator;
        }

        private new string ErrorMessage { get; set; }

        private  DateTime? CompareDate { get; set; }

        private string CompareOperator { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            var compareData = DateTime.Now;

            if (CompareDate.HasValue)
                compareData = CompareDate.Value;


            switch (CompareOperator)
            {
                case "=":
                {
                    return date.Date == compareData.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }
                case ">":
                {
                    return date.Date > compareData.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }
                case ">=":
                {
                    return date.Date >= compareData.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }
                case "<":
                {
                    return date.Date < compareData.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }
                case "<=":
                {
                    return date.Date <= compareData.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
                }

                default:
                    return new ValidationResult(ErrorMessage);
            }
        }
    }
}
