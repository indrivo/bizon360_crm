using System;
using System.Collections.Generic;

namespace GR.Crm.Payments.Abstractions.ViewModels.PaymentDashboardViewModel
{
    public class PaymentDashboardViewModel
    {

        public virtual List<PaymentData> Payments  { get; set; } = new List<PaymentData>();

        public virtual List<PaymentData> Clients { get; set; } = new List<PaymentData>();

        public virtual List<PaymentData> Cards { get; set; } = new List<PaymentData>();

        public virtual List<PaymentData> TotalQuantity { get; set; } = new List<PaymentData>();

        public virtual List<PaymentData> AverageUnitPrice { get; set; } = new List<PaymentData>();
    }


    public class PaymentData
    {
        public virtual string GeoPosition { get; set; }

        public virtual IEnumerable<ListValue> ListValues { get; set; }

        public virtual decimal Value { get; set; }

        public virtual decimal ValueProgress { get; set; }
    }

    public class ListValue
    {
        public virtual DateTime  Data{ get; set; }
        public virtual decimal Value { get; set; }
    }
}
