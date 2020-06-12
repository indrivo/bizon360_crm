using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class RegionViewModel: BaseModel
    {
        /// <summary>
        /// Region name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual Guid CountryId { get; set; }

    }
}
