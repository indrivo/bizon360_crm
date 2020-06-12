using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Reports.Abstraction.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        /// <summary>
        /// Add Crm report module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmReportModule<TService>(this IServiceCollection services)
             where TService : class, ICrmReportService
        {
            services.AddGearTransient<ICrmReportService, TService>();
            return services;
        }

    }
}
