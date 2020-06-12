using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class OrganizationAddressViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }
        

        /// <summary>
        /// City id
        /// </summary>
        [Required]
        public virtual Guid CityId { get; set; }

        /// <summary>
        /// Street
        /// </summary>
        [MaxLength(128)]
        public virtual string Street { get; set; }

        /// <summary>
        /// Zip code
        /// </summary>
        [MaxLength(28)]
        public virtual string Zip { get; set; }
    }
}
