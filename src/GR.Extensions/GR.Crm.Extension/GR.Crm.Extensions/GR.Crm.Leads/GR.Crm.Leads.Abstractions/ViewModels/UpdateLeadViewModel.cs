using System;
using System.ComponentModel.DataAnnotations;
using GR.Core.Extensions;
using GR.Crm.Leads.Abstractions.Extensions;
using GR.Crm.Leads.Abstractions.Helpers;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class UpdateLeadViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public virtual DateTime Created { get; set; }


        /// <summary>
        /// Lead name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Organization id 
        /// </summary>
        [Required]
        public virtual Guid? OrganizationId { get; set; }


        /// <summary>
        /// Value
        /// </summary>
        public virtual decimal Value { get; set; } = 0;

        /// <summary>
        /// Currency
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Clarification deadline
        /// </summary>
        [ValidationUpdateLeadDataTime("End date is less Start date")]
        public virtual DateTime? DeadLine { get; set; }

        /// <summary>
        /// Lead state id
        /// </summary>
        [Required]
        public virtual Guid? LeadStateId { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        public virtual Guid? ProductId { get; set; }

        /// <summary>
        /// Stage id
        /// </summary>
        [Required]
        public virtual Guid StageId { get; set; }

        /// <summary>
        /// Clarification deadline
        /// </summary>
        public virtual DateTime? ClarificationDeadline { get; set; }


        /// <summary>
        /// Contact id
        /// </summary>
        public virtual Guid? ContactId { get; set; }

        /// <summary>
        /// ProductType reference
        /// </summary>
        public virtual Guid? ProductTypeId { get; set; }

        /// <summary>
        /// ServiceType reference
        /// </summary>
        public virtual Guid? ServiceTypeId { get; set; }

        /// <summary>
        /// SolutionType reference
        /// </summary>
        public virtual Guid? SolutionTypeId { get; set; }

        /// <summary>
        /// Source reference
        /// </summary>
        public virtual Guid? SourceId { get; set; }

        /// <summary>
        /// TechnologyType reference
        /// </summary>
        public virtual Guid? TechnologyTypeId { get; set; }


        /// <summary>
        /// Description 
        /// </summary>
        public virtual string Description { get; set; }
    }
}
