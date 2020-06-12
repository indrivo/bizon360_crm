using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Organizations.Abstractions.Enums;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Organization : BaseModel
    {
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
        /// Client type
        /// </summary>
        [Required]
        public virtual OrganizationType ClientType { get; set; } = OrganizationType.Prospect;

        /// <summary>
        /// Bank 
        /// </summary>
        public virtual string Bank { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public virtual string Email { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string Phone { get; set; }

        /// <summary>
        /// Web site
        /// </summary>
        [MaxLength(50)]
        public virtual string WebSite { get; set; }

        /// <summary>
        /// Fiscal code
        /// </summary>
        [MaxLength(15)]
        [MinLength(6)]
        public virtual string FiscalCode { get; set; }

        /// <summary>
        /// IBAN code
        /// </summary>
        [MaxLength(128)]
        public virtual string IBANCode { get; set; }

        /// <summary>
        /// cod swift
        /// </summary>
        public virtual string CodSwift { get; set; }

        /// <summary>
        /// cod TVA
        /// </summary>
        public virtual string VitCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// List contacts
        /// </summary>
        public virtual IEnumerable<Contact> Contacts { get; set; } = new List<Contact>();

        /// <summary>
        /// Industry Id
        /// </summary>
        public virtual Guid? IndustryId { get; set; }


        /// <summary>
        /// Industry
        /// </summary>
        public Industry Industry { get; set; }

        /// <summary>
        /// Employees 
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        public virtual Guid? EmployeeId { get; set; }

        /// <summary>
        /// Addresses 
        /// </summary>
        public virtual IEnumerable<OrganizationAddress> Addresses { get; set; }

    }
}
