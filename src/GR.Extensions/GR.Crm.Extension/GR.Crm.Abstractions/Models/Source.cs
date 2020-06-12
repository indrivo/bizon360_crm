using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models
{
    public class Source : BaseModel
    {
        /// <summary>
        /// Source name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
