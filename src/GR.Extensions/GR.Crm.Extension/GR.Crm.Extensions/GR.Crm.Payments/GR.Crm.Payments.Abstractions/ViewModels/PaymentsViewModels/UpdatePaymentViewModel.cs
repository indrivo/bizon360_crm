using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels
{
    public class UpdatePaymentViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Transaction date
        /// </summary>
        public virtual DateTime DateTransaction { get; set; }


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
        [Required]
        public virtual decimal TVA { get; set; }

        /// <summary>
        /// Total price without TVA
        /// </summary>
        public virtual decimal TotalPriceWithoutTVA { get; set; }


        /// <summary>
        /// Unit price without tva
        /// </summary>
        [Required]
        public virtual decimal UnitPriceWithoutTVA { get; set; }

        /// <summary>
        /// Quantity 
        /// </summary>
        [Required]
        public virtual decimal Quantity { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public virtual string Currency { get; set; }


        /// <summary>
        /// Payment destination
        /// </summary>
        public virtual string PaymentDestination { get; set; }

        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        [Required]
        public virtual Guid ProductId { get; set; }

        /// <summary>
        /// Work category id
        /// </summary>
        [Required]
        public virtual Guid WorkCategoryId { get; set; }


        /// <summary>
        /// Payment code id
        /// </summary>
        [Required]
        public virtual Guid PaymentCodeId { get; set; }
    }
}
