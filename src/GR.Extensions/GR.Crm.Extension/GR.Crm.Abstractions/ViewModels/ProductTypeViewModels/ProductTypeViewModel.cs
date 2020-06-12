using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Abstractions.ViewModels.ProductTypeViewModels
{
    public class ProductTypeViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
