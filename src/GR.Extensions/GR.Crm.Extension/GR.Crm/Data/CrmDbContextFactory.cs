using GR.Core.Helpers.DbContexts;
using Microsoft.EntityFrameworkCore.Design;

namespace GR.Crm.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Context factory design
    /// </summary>
    public class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
    {
        /// <inheritdoc />
        /// <summary>
        /// For creating migrations
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CrmDbContext CreateDbContext(string[] args)
        {
            return DbContextFactory<CrmDbContext, CrmDbContext>.CreateFactoryDbContext();
        }
    }
}
