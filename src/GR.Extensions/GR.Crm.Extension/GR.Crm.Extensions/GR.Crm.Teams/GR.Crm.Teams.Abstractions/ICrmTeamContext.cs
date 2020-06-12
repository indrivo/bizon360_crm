using GR.Crm.Abstractions;
using GR.Crm.Teams.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Teams.Abstractions
{
    public interface ICrmTeamContext : ICrmContext
    {
        /// <summary>
        /// Team
        /// </summary>
        DbSet<Team> Teams { get; set; }

        /// <summary>
        /// Team members
        /// </summary>
        DbSet<TeamMember> TeamMembers { get; set; }

        /// <summary>
        /// Team role
        /// </summary>
        DbSet<TeamRole> TeamRoles { get; set; }
    }
}
