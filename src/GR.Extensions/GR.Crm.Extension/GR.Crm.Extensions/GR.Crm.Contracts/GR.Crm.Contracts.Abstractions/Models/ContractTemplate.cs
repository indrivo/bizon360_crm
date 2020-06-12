using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Contracts.Abstractions.Models
{
    public class ContractTemplate : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [MaxLength(200)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Contract template sections
        /// </summary>
        public virtual ICollection<ContractSection> Sections { get; set; } = new List<ContractSection>();
    }
}
