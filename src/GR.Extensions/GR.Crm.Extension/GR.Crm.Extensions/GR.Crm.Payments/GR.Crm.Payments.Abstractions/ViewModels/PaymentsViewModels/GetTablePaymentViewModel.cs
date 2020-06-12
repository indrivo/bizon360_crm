using System;
using System.Collections.Generic;
using System.Text;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Payments.Abstractions.Models;

namespace GR.Crm.Payments.Abstractions.ViewModels.PaymentsViewModels
{
    public class GetTablePaymentViewModel : PaymentMapped
    {
        public virtual string OrganizationName { get; set; }

        public virtual string Idno { get; set; }

        public virtual DateTime PaymentDate { get; set; }

        public virtual int GeoPosition { get; set; }

    }
}
