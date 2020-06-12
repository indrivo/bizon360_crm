using GR.Crm.Abstractions;
using GR.Crm.Contracts.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Contracts.Abstractions
{
    public interface IContractsContext : ICrmContext
    {
        /// <summary>
        /// Templates
        /// </summary>
        DbSet<ContractTemplate> ContractTemplates { get; set; }

        /// <summary>
        /// Sections
        /// </summary>
        DbSet<ContractSection> ContractSections { get; set; }
    }
}