using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Abstractions.ViewModels.SourceViewModels
{
    public class SourceViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
