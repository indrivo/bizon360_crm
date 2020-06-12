using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.PipeLines.Abstractions.ViewModels
{
    public sealed class OrderStagesViewModel
    {
        [Required]
        public Guid StageId { get; set; }
        
        [Required]
        public int Order { get; set; }
    }
}
