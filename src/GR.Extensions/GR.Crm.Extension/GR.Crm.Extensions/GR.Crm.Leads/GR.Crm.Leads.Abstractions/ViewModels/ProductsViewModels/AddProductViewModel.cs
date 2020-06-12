using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels.ProductsViewModels
{
    public class AddProductViewModel
    {
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        public virtual string Sku { get; set; }

        /// <summary>
        /// Bank account 
        /// </summary>
        [Required]
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public virtual decimal Price { get; set; } = 0;

        /// <summary>
        /// Vat
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        public virtual decimal Commission { get; set; } = 0;
    }
}
