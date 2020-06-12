using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.PipeLines.Abstractions.ViewModels
{
    public class AddStageViewModel
    {
        /// <summary>
        /// PipeLine id
        /// </summary>
        [Required]
        public virtual Guid PipeLineId { get; set; }

        /// <summary>
        /// Name 
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Tem 
        /// </summary>
        [Required]
        [Range(1, 180, ErrorMessage = "Please enter valid integer Number (1 - 180)")]
        public virtual int Term { get; set; }

        /// <summary>
        /// WorkFlow Id
        /// </summary>

        public virtual Guid WorkFlowStateId { get; set; }

       /// <summary>
       /// Is system stage (No editable)
       /// </summary>
       public virtual bool IsSystem { get; set; }
    }
}
