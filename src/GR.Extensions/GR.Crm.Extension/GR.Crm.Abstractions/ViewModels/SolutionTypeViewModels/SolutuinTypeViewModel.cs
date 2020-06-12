using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Abstractions.ViewModels.SolutionTypeViewModels
{
    public class SolutionTypeViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
