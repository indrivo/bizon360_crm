using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models
{
    public class TechnologyType: BaseModel
    {
        /// <summary>
        /// Technology type name 
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
