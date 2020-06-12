using System;
using System.Collections.Generic;
using System.Text;
using GR.Crm.Abstractions;
using GR.Crm.Payments.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Payments.Abstractions
{
    public interface IPaymentContext : ICrmContext
    {
        /// <summary>
        /// Payments
        /// </summary>
        DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Payments Mappers
        /// </summary>
        DbSet<PaymentMapped> PaymentMappers { get; set; }


        /// <summary>
        /// Payments Code
        /// </summary>
        DbSet<PaymentCode> PaymentCodes { get; set; }
    }
}
