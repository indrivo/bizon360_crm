using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.ViewModels
{
    public class GetTeamMemberViewModel: BaseModel
    {
        /// <summary>
        /// User id
        /// </summary>
        [Required]
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public virtual string LastName { get; set; }

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

        /// <summary>
        /// TeamRole
        /// </summary>
        public virtual TeamRoleViewModel TeamRole { get; set; }
    }
}
