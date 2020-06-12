using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class ContactWebProfile : BaseModel
    {
        /// <summary>
        /// Web profile Id
        /// </summary>
        [Required]
        public virtual Guid WebProfileId { get; set; }

        /// <summary>
        /// Web profile
        /// </summary>
        public virtual WebProfile WebProfile { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string UserName { get; set; }


        /// <summary>
        /// url
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string Url { get; set; }


        /// <summary>
        /// Contact id
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }


        /// <summary>
        /// Contact
        /// </summary>
        public virtual Contact Contact { get; set; }
    }
}
