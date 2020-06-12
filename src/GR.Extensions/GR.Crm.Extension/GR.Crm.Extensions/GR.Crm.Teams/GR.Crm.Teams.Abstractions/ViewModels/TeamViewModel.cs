using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.ViewModels
{
    public class TeamViewModel : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [MaxLength(128)]
        [MinLength(2)]
        public virtual string Name { get; set; }

        public virtual IEnumerable<GetTeamMemberViewModel> TeamMembers { get; set; } = new List<GetTeamMemberViewModel>();
    }
}
