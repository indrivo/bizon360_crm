using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.Teams.Abstractions.Models;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class Lead : BaseModel
    {
        /// <summary>
        /// Lead name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(256)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Organization reference
        /// </summary>
        public virtual Organization Organization { get; set; }
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// PipeLine reference
        /// </summary>
        public virtual PipeLine PipeLine { get; set; }
        [Required]
        public virtual Guid PipeLineId { get; set; }

        /// <summary>
        /// Stage reference
        /// </summary>
        public virtual Stage Stage { get; set; }
        [Required]
        public virtual Guid StageId { get; set; }

        /// <summary>
        /// Stage reference
        /// </summary>
        public virtual LeadState LeadState { get; set; }
        [Required]
        public virtual Guid LeadStateId { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public virtual decimal Value { get; set; } = 0;

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual Currency Currency { get; set; }
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        [Required]
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Offer deadline
        /// </summary>
        public virtual DateTime? StageDeadLine { get; set; }

        /// <summary>
        /// Clarification deadline
        /// </summary>
        public virtual DateTime? DeadLine { get; set; }

        /// <summary>
        /// Team reference
        /// </summary>
        public virtual Team Team { get; set; }
        public virtual Guid? TeamId { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        public virtual Guid? ProductId { get; set; }

        /// <summary>
        /// Check if has team
        /// </summary>
        /// <returns></returns>
        public bool HasTeam() => TeamId != null;

        /// <summary>
        /// Clarification deadline
        /// </summary>
        public virtual DateTime? ClarificationDeadline { get; set; }


        /// <summary>
        /// Contact reference
        /// </summary>
        public virtual Contact Contact { get; set; }
        public virtual Guid? ContactId { get; set; }

        /// <summary>
        /// ProductType reference
        /// </summary>
        public virtual ProductType ProductType { get; set; }
        public virtual Guid? ProductTypeId { get; set; }

        /// <summary>
        /// ServiceType reference
        /// </summary>
        public virtual ServiceType ServiceType { get; set; }
        public virtual Guid? ServiceTypeId { get; set; }

        /// <summary>
        /// SolutionType reference
        /// </summary>
        public virtual SolutionType SolutionType { get; set; }
        public virtual Guid? SolutionTypeId { get; set; }

        /// <summary>
        /// Source reference
        /// </summary>
        public virtual Source Source { get; set; }
        public virtual Guid? SourceId { get; set; }

        /// <summary>
        /// TechnologyType reference
        /// </summary>
        public virtual TechnologyType TechnologyType { get; set; }
        public virtual Guid? TechnologyTypeId { get; set; }

        /// <summary>
        /// Country reference
        /// </summary>
        public virtual CrmCountry Country { get; set; }
        public virtual Guid? CountryId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        public virtual string Description { get; set; }
    }
}
