using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class AddCityViewModel
    {
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Region id
        /// </summary>
        [Required]
        public virtual Guid RegionId {get; set; }
    }
}
