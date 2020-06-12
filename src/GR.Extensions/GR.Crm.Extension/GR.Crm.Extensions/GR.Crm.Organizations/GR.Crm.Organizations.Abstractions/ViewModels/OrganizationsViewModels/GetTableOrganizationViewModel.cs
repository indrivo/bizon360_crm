using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
    public class GetTableOrganizationViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Client Name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }


        /// <summary>
        /// Brand
        /// </summary>
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Brand { get; set; }


        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Client type
        /// </summary>
        [Required]
        public virtual OrganizationType ClientType { get; set; } = OrganizationType.Prospect;

        /// <summary>
        /// List contacts
        /// </summary>
        public virtual IEnumerable<Contact> Contacts { get; set; } = new List<Contact>();


        /// <summary>
        /// Lead count
        /// </summary>
        public virtual int LeadCount { get; set; }

        /// <summary>
        /// Lead count
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// work categories
        /// </summary>
        public virtual IEnumerable<string> WorkCategories { get; set; } = new List<string>();
    }
}
