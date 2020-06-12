using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class CountryViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
