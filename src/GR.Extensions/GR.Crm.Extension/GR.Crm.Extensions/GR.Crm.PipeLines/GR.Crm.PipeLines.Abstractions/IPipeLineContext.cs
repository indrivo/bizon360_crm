using GR.Crm.Abstractions;
using GR.Crm.PipeLines.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.PipeLines.Abstractions
{
    public interface IPipeLineContext : ICrmContext
    {
        /// <summary>
        /// PipeLines
        /// </summary>
        DbSet<PipeLine> PipeLines { get; set; }

        /// <summary>
        /// Stages
        /// </summary>
        DbSet<Stage> Stages { get; set; }
    }
}