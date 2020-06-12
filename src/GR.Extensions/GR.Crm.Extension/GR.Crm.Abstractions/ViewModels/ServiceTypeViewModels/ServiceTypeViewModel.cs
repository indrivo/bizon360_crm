using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Abstractions.ViewModels.ServiceTypeViewModels
{
    public class ServiceTypeViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
