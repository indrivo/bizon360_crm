using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.PipeLines.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Crm PipeLine module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmPipeLineModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmPipeLineService
            where TContext : DbContext, IPipeLineContext
        {
            services.AddGearTransient<ICrmPipeLineService, TService>();
            services.AddTransient<IPipeLineContext, TContext>();
            return services;
        }
    }
}
