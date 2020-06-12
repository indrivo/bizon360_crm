using GR.Core.Abstractions;
using GR.Crm.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Abstractions
{
    public interface ICrmContext : IDbContext
    {
        /// <summary>
        /// Job position
        /// </summary>
        DbSet<JobPosition> JobPositions { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        DbSet<Source> Sources { get; set; }

        /// <summary>
        /// Solution Types
        /// </summary>
        DbSet<SolutionType> SolutionTypes { get; set; }

        /// <summary>
        /// Technology Types
        /// </summary>
        DbSet<TechnologyType> TechnologyTypes { get; set; }

        /// <summary>
        /// Service Types
        /// </summary>
        DbSet<ServiceType> ServiceTypes { get; set; }

        /// <summary>
        ///  Product Types
        /// </summary>
        DbSet<ProductType> ProductTypes { get; set; }

        /// <summary>
        /// Currencies
        /// </summary>
        DbSet<Currency> Currencies { get; set; }
    }
}