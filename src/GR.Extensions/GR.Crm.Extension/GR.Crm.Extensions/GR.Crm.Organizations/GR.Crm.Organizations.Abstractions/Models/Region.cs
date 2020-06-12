using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Region : BaseModel
    {
        /// <summary>
        /// Country reference
        /// </summary>
        public virtual CrmCountry Country { get; set; }

        public virtual Guid CountryId { get; set; }

        /// <summary>
        /// Region name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Cities
        /// </summary>
        public virtual IEnumerable<City> Cities { get; set; }
    }
}
