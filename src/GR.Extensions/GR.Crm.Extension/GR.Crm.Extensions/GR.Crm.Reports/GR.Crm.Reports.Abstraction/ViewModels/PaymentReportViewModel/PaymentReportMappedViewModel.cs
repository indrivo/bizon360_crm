using System;

namespace GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel
{
    public class PaymentReportMappedViewModel
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual DateTime DateTransaction { get; set; }

        public virtual string GeoPosition { get; set; }

        public virtual decimal TotalPrice { get; set; }

        public virtual decimal Quantity { get; set; }
    }
}
