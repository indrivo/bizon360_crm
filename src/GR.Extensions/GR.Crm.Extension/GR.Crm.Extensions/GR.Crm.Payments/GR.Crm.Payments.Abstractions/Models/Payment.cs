using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Payments.Abstractions.Models
{
    public class Payment: BaseModel
    {
        /// <summary>
        /// Document number
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Transaction date
        /// </summary>
        [Required]
        public virtual DateTime DateTransaction { get; set; }

        /// <summary>
        /// Cod fiscal/ IDNO
        /// </summary>
        [Required]
        public virtual string FiscalCode { get; set; }

        /// <summary>
        /// Bank account
        /// </summary>
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public virtual string Currency { get; set; }
        
        /// <summary>
        /// Payment destination
        /// </summary>
        public virtual string PaymentDestination { get; set; }

        /// <summary>
        /// Total price
        /// </summary>
        public virtual decimal TotalPrice { get; set; }

        /// <summary>
        /// Total TVA
        /// </summary>

        public virtual decimal TotalTVA { get; set; }

        /// <summary>
        /// TVA
        /// </summary>
        public virtual decimal TVA { get; set; }

        /// <summary>
        /// Total price without TVA
        /// </summary>
        public virtual decimal TotalPriceWithoutTVA { get; set; }


        /// <summary>
        /// Unit price without tva
        /// </summary>
        public virtual decimal UnitPriceWithoutTVA { get; set; }

        /// <summary>
        /// Quantity 
        /// </summary>
        public virtual decimal  Quantity { get; set; }

    }
}
