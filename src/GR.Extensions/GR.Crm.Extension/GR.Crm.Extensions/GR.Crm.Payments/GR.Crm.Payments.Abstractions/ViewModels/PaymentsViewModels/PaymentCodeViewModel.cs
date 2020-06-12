using System;
using System.ComponentModel.DataAnnotations;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels
{
    public class PaymentCodeViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set;}

        /// <summary>
        /// Code
        /// </summary>
        [Required]
        public virtual string Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Work category
        /// </summary>
        [Required]
        public virtual Guid WorkCategoryId { get; set; }

    }
}
