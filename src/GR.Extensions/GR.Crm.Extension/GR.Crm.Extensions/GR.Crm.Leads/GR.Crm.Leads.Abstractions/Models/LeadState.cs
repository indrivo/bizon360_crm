using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Helpers.Global;

namespace GR.Crm.Leads.Abstractions.Models
{
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public class LeadState : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// State style class
        /// </summary>
        public virtual string StateStyleClass { get; set; }

        /// <summary>
        /// Is system state
        /// </summary>
        public virtual bool IsSystem { get; set; } = false;


        /// <summary>
        /// Order
        /// </summary>
        public virtual int Order { get; set; } = 1;
    }
}
