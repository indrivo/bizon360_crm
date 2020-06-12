using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Teams.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Register Crm Team module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmTeamModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmTeamService
            where TContext : DbContext, ICrmTeamContext
        {
            services.AddGearTransient<ICrmTeamService, TService>();
            services.AddTransient<ICrmTeamContext, TContext>();
            return services;
        }
    }
}
