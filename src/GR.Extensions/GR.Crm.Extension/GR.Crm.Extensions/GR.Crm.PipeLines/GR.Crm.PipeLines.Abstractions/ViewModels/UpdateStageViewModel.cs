using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.PipeLines.Abstractions.ViewModels
{
    public class UpdateStageViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Term
        /// </summary>
        [Required]
        [Range(1, 180, ErrorMessage = "Please enter valid integer Number (1 - 180)")]
        public virtual int Term { get; set; }
    }
}
