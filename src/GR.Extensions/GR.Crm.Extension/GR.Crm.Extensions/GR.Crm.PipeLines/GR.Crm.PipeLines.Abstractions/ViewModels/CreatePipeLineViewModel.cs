using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.PipeLines.Abstractions.ViewModels
{
    public class CreatePipeLineViewModel
    {
        /// <summary>
        /// PipeLine name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Workflow
        /// </summary>
        public virtual Guid? WorkFlowId { get; set; }
    }
}
