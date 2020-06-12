using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.ViewModels
{
    public class TeamMemberViewModel 
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

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
        /// TeamRole id
        /// </summary>
        [Required]
        public virtual Guid TeamRoleId { get; set; }
      
    }
}
