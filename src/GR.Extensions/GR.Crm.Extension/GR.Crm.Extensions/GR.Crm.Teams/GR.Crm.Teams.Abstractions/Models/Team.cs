using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.Models
{
    public class Team : BaseModel 
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [MaxLength(128)]
        [MinLength(2)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Team members 
        /// </summary>
        public virtual IEnumerable<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}
