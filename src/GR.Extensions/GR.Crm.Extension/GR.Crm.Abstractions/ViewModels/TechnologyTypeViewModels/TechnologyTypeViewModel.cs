using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Abstractions.ViewModels.TechnologyTypeViewModels
{
   public  class TechnologyTypeViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
