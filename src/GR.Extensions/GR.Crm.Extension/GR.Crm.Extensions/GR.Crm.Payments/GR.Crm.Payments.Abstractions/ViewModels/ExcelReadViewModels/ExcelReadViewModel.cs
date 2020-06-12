using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Payments.Abstractions.ViewModels.ExcelReadViewModels
{
    public class ExcelReadViewModel
    {
        /// <summary>
        /// Read line 
        /// </summary>
        public virtual int Line { get; set; }

        /// <summary>
        /// Date transaction
        /// </summary>
        public virtual DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Document number
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Type transaction
        /// </summary>
        public virtual string TypeTransaction { get; set; }

        /// <summary>
        /// Bank Account
        /// </summary>
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// Fiscal code
        /// </summary>
        public virtual string FiscalCode { get; set; }

        /// <summary>
        /// Organization name
        /// </summary>
        public virtual string OrganizationName { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public virtual string Currency { get; set; }

        /// <summary>
        /// Total amount
        /// </summary>
        public virtual decimal? TotalAmount { get; set; }

        /// <summary>
        /// Payment destination
        /// </summary>
        public virtual string PaymentDestination { get; set; }

    }
}
