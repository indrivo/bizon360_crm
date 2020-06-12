using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.ViewModels.EmployeesViewModels
{
    public class EmployeeViewModel 
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Employees interval
        /// </summary>
        [Required]
        public virtual string Interval { get; set; }
    }
}
