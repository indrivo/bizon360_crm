using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.Models
{
    public class TeamMember : BaseModel
    {

        /// <summary>
        /// User id
        /// </summary>
        [Required]
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Team id
        /// </summary>
        [Required]
        public virtual Guid TeamId { get; set; }

        /// <summary>
        /// Team
        /// </summary>
        public virtual Team Team { get; set; }

        /// <summary>
        /// TeamRole id
        /// </summary>
        [Required]
        public virtual Guid? TeamRoleId { get; set; }

        /// <summary>
        /// TeamRole
        /// </summary>
        public virtual TeamRole TeamRole { get; set; }

    }
}
