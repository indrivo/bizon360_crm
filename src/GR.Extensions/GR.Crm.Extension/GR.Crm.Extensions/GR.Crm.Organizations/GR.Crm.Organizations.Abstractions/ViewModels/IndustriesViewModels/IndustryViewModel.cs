using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels
{
    public class IndustryViewModel 
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
