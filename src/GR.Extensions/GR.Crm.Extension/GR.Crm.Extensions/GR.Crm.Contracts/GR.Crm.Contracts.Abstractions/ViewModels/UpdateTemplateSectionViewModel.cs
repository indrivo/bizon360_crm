using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Contracts.Abstractions.ViewModels
{
    public class UpdateTemplateSectionViewModel
    {
        /// <summary>
        /// Section id
        /// </summary>
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Template value with tokens
        /// </summary>
        public virtual string TemplateValue { get; set; }
    }
}
