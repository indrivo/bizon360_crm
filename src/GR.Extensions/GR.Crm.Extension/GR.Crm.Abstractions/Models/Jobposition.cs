using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models
{
    public  class JobPosition : BaseModel
    {
        /// <summary>
        /// Job position
        /// </summary>
         [Required]
        public virtual string Name { get; set; }
    }
}
