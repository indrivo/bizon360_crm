using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Contracts.Abstractions.ViewModels
{
    public class OrderSectionViewModel
    {
        [Required]
        public Guid SectionId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
