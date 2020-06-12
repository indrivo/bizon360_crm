using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.PipeLines.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadContext<TLead> : ICrmOrganizationContext, IPipeLineContext
        where TLead : Lead
    {
        /// <summary>
        /// Leads
        /// </summary>
        DbSet<TLead> Leads { get; set; }

        /// <summary>
        /// States
        /// </summary>
        DbSet<LeadState> States { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        DbSet<Agreement> Agreements { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        DbSet<Product> Products { get; set; }
    }
}