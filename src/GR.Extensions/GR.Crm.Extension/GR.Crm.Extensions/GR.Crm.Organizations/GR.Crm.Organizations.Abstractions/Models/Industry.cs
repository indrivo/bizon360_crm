using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Industry : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
