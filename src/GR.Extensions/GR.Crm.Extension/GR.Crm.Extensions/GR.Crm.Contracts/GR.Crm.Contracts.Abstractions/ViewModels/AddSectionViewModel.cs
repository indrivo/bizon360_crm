using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Contracts.Abstractions.ViewModels
{
    public class AddSectionViewModel
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
        /// Contract template reference
        /// </summary>
        [Required]
        public virtual Guid ContractTemplateId { get; set; }
    }
}
