using System;
using System.ComponentModel.DataAnnotations;
using GR.Crm.Organizations.Abstractions.Enums;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
    public class OrganizationViewModel
    {

        public virtual Guid? Id { get; set; }

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
        [MaxLength(8)]
        [MinLength(8)]
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
        /// Industry Id
        /// </summary>
        public virtual Guid? IndustryId { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        public virtual Guid? EmployeeId { get; set; }
    }
}
