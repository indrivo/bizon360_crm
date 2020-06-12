using System;
using System.Collections.Generic;
using System.Text;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Teams.Abstractions.Models;

namespace GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels
{
    public class ReportLeadMappedViewModel
    {
        public virtual Guid Id { get; set; }

        public virtual Guid OrganizationId { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual string GeoPosition { get; set; }

        public virtual string Stage { get; set; }

        public virtual string State { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual int UnitsNumber { get; set; }

        public virtual decimal Value { get; set; }

        public virtual decimal Commission { get; set; }

        public virtual string Pipeline { get; set; }
    }
}
