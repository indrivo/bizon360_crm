using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Teams.Abstractions.ViewModels
{
    public class TeamRoleViewModel : BaseModel
    {

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
