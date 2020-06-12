using System;
using System.Collections.Generic;
using System.Text;
using GR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Dashboard.Abstractions.Extensions
{
    public static class ServiceCollectionExtension
    {

        /// <summary>
        /// Add dashboard service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmDashboardModule<TService>(this IServiceCollection services)
            where TService : class, ICrmDashboardService
        {
            services.AddGearTransient<ICrmDashboardService, TService>();
            return services;
        }
    }
}
