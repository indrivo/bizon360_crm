using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GR.Crm.Leads.Abstractions.ViewModels;

namespace GR.Crm.Leads.Abstractions.Helpers
{
   public  class ValidationUpdateLeadDataTime : ValidationAttribute
    {


        public ValidationUpdateLeadDataTime(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        private new string ErrorMessage { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var task = (UpdateLeadViewModel)validationContext.ObjectInstance;

            return date.Date >= task.Created.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

    }
}
