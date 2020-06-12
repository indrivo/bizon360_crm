using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Reports.Abstraction.ViewModels.AgreementViewModels
{
    public class ReportAgreementViewModel
    {
        public virtual object GroupKeys { get; set; }

        public virtual int Count { get; set; }


        public virtual decimal SumValue { get; set; }
    }
}
