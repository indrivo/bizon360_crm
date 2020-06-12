using System;
using System.Collections.Generic;
using GR.Core;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Teams.Abstractions.ViewModels;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class GetLeadsViewModel : Lead
    {

        public virtual string OrganizationName { get; set; }

        public virtual IEnumerable<GetTeamMemberViewModel> LeadMembers { get; set; }

        public virtual Guid? OwnerId { get; set; }
    }
}
