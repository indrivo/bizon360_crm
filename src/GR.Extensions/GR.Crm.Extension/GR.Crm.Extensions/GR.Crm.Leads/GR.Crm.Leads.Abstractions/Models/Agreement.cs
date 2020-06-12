using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class Agreement : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Lead
        /// </summary>
        public virtual Lead Lead { get; set; }

        /// <summary>
        /// Lead id
        /// </summary>
        [Required]
        public virtual Guid LeadId { get; set; }

        /// <summary>
        /// Organization 
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Organization contact
        /// </summary>
        public virtual Contact Contact { get; set; }

        /// <summary>
        /// Organization address is
        /// </summary>

        public virtual Guid? OrganizationAddressId { get; set; }

        /// <summary>
        /// Organization address
        /// </summary>

        public virtual OrganizationAddress OrganizationAddress { get; set; }

        /// <summary>
        /// organization contact id
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Contract template id
        /// </summary>
        public virtual Guid ContractTemplateId { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [Required]
        public virtual decimal Commission { get; set; }

        /// <summary>
        /// Total value
        /// </summary>
        public virtual decimal Values { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        [Required]
        public virtual Guid ProductId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        public virtual string Description { get; set; }

    }
}
