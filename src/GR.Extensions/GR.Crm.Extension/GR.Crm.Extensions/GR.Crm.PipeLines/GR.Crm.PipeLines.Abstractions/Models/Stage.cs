using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Helpers.Global;

namespace GR.Crm.PipeLines.Abstractions.Models
{
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public class Stage : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// PipeLine reference
        /// </summary>
        public virtual PipeLine PipeLine { get; set; }
        public virtual Guid PipeLineId { get; set; }

        /// <summary>
        /// Workflow state
        /// </summary>
        public virtual Guid? WorkFlowStateId { get; set; }

        /// <summary>
        /// Order
        /// </summary>
        public virtual int Order { get; set; } = 1;

        /// <summary>
        /// Term 
        /// </summary>
        public virtual int? Term { get; set; }


        /// <summary>
        /// Is system stage
        /// </summary>
        public virtual bool IsSystem { get; set; }

        /// <summary>
        /// Has state
        /// </summary>
        /// <returns></returns>
        public virtual bool HasWorkflowState() => WorkFlowStateId != null;
    }
}
