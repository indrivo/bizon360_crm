using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.Models
{
    public class TeamRole: BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
