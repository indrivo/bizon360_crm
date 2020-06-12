using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Contracts.Abstractions.ViewModels
{
    public class AddTemplateViewModel
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
    }
}
