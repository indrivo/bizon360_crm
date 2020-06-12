using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel
{
    public class ReportPaymentViewModel
    {
        public virtual object GroupKeys { get; set; }

        public virtual int Count { get; set; }

        public virtual decimal SumQuantity { get; set; }

        public virtual decimal SumValue { get; set; }

        public virtual decimal AverageCommission { get; set; }
    }
}
