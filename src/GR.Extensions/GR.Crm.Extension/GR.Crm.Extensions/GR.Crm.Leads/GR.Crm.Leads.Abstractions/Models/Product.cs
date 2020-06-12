using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class Product: BaseModel
    {

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
        public virtual decimal Price { get; set; }

        /// <summary>
        /// Vat
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        public virtual decimal Commission { get; set; }

    }
}
