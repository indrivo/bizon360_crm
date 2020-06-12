using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels
{
    public class AddAgreementViewModel
    {


        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Lead id
        /// </summary>
        [Required]
        public virtual Guid LeadId { get; set; }

        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// organization contact id
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }

        /// <summary>
        /// Organization address is
        /// </summary>
        [Required]
        public virtual Guid? OrganizationAddressId { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Contract template id
        /// </summary>
        [Required]
        public virtual Guid ContractTemplateId { get; set; }


        /// <summary>
        /// Commission
        /// </summary>
        [Required]
        public virtual decimal Commission { get; set; }

        /// <summary>
        /// Total value
        /// </summary>
        public virtual decimal Values { get; set; } = 0;


        /// <summary>
        /// Product id
        /// </summary>
        //[Required]
        public virtual Guid ProductId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
    }
}
