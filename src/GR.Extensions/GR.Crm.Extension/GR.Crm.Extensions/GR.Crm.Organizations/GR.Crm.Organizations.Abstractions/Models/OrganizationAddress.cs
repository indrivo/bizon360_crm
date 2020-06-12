using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class OrganizationAddress: BaseModel
    {

        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
         public virtual Organization Organization { get; set; }

        /// <summary>
        /// City id
        /// </summary>
        [Required]
        public virtual Guid CityId { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public virtual City City { get; set; }

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
