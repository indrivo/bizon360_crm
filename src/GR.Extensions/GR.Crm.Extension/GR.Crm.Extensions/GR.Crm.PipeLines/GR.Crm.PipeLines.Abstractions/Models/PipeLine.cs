using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Helpers.Global;

namespace GR.Crm.PipeLines.Abstractions.Models
{
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public class PipeLine : BaseModel
    {
        /// <summary>
        /// PipeLine name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// PipeLine stages
        /// </summary>
        public virtual ICollection<Stage> Stages { get; set; } = new List<Stage>();

        /// <summary>
        /// Workflow
        /// </summary>
        public virtual Guid? WorkFlowId { get; set; }

        /// <summary>
        /// Has workflow
        /// </summary>
        /// <returns></returns>
        public virtual bool HasWorkflow() => WorkFlowId.HasValue;
    }
}
