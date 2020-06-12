using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Employee : BaseModel
    {
        /// <summary>
        /// Employees interval
        /// </summary>
        [Required]
        public virtual string Interval { get; set; }
    }
}
