using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Contracts.Abstractions.Models
{
    public class ContractSection : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Template value with tokens
        /// </summary>
        public virtual string TemplateValue { get; set; }

        /// <summary>
        /// Order section
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// Contract template reference
        /// </summary>
        [Required]
        public virtual Guid ContractTemplateId { get; set; }
        public virtual ContractTemplate ContractTemplate { get; set; }
    }
}
