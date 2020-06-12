using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class City : BaseModel
    {
        /// <summary>
        /// City name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Region id
        /// </summary>
        [Required]
        public virtual Guid? RegionId { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public virtual Region Region { get; set; }
    }
}
