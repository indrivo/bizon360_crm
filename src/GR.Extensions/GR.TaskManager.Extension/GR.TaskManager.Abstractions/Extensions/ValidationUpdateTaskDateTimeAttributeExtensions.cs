using System;
using System.ComponentModel.DataAnnotations;
using GR.TaskManager.Abstractions.Models.ViewModels;

namespace GR.TaskManager.Abstractions.Extensions
{
    public class ValidationUpdateTaskDateTimeAttributeExtensions : ValidationAttribute
    {

        public ValidationUpdateTaskDateTimeAttributeExtensions(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        private new string ErrorMessage { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var task = (UpdateTaskViewModel)validationContext.ObjectInstance;

            return date.Date >= task.StartDate.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

    }
}
