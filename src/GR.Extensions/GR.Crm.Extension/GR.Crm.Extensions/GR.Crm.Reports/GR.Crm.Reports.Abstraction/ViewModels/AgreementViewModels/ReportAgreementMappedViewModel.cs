using System;
using System.Collections.Generic;
using System.Text;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Reports.Abstraction.ViewModels.AgreementViewModels
{
    public class ReportAgreementMappedViewModel
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual string Author { get; set; }

        public virtual string GeoPosition { get; set; }

        public virtual decimal Values { get; set; }

        public virtual string Agent { get; set; }
    }
}
