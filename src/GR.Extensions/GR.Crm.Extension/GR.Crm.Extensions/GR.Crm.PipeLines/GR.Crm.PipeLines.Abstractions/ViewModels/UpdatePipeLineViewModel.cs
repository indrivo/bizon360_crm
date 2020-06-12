using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.PipeLines.Abstractions.ViewModels
{
    public class UpdatePipeLineViewModel
    {
        /// <summary>
        /// PipeLine id
        /// </summary>
        [Required]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Pipeline name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
    }
}
