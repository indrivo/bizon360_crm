using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models
{
    public class SolutionType: BaseModel
    {
        /// <summary>
        /// Source type name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
