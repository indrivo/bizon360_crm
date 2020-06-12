using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Contracts.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add contracts DI services
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmContractsModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmContractsService
            where TContext : DbContext, IContractsContext
        {
            services.AddGearTransient<ICrmContractsService, TService>();
            services.AddTransient<IContractsContext, TContext>();
            return services;
        }
    }
}
