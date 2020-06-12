using System;
using System.ComponentModel.DataAnnotations;
using GR.TaskManager.Abstractions.Models.ViewModels;

namespace GR.TaskManager.Abstractions.Extensions
{
    public class ValidationTaskDateTimeAttributeExtensions : ValidationAttribute
    {

        public ValidationTaskDateTimeAttributeExtensions(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        private new string ErrorMessage { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var task = (TaskBaseModel)validationContext.ObjectInstance;

            return date.Date >= task.StartDate.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

    }
}
