using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Teams.Abstractions.ViewModels
{
    public class AddTeamViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual IEnumerable<TeamMemberAdd> ListMembers { get; set; } = new List<TeamMemberAdd>();
    }


    public class TeamMemberAdd
    {
        [Required]
        public virtual Guid UserId { get; set; }
        [Required]
        public virtual Guid RoleId { get; set; }
    }

}
