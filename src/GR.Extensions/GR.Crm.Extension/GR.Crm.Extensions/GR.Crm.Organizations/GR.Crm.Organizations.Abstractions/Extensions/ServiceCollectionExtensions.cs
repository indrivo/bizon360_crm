using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Organizations.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add contact module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmContactModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmContactService
            where TContext : DbContext, ICrmOrganizationContext
        {
            services.AddGearTransient<ICrmContactService, TService>();
            services.AddTransient<ICrmOrganizationContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add organization module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmOrganizationModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmOrganizationService
            where TContext : DbContext, ICrmOrganizationContext
        {
            services.AddGearTransient<ICrmOrganizationService, TService>();
            services.AddTransient<ICrmOrganizationContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add Industry module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmIndustryModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmIndustryService
            where TContext : DbContext, ICrmOrganizationContext
        {
            services.AddGearTransient<ICrmIndustryService, TService>();
            services.AddTransient<ICrmOrganizationContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add Employees module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmEmployeesModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmEmployeeService
            where TContext : DbContext, ICrmOrganizationContext
        {
            services.AddGearTransient<ICrmEmployeeService, TService>();
            services.AddTransient<ICrmOrganizationContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add Employees organization address module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmAddressModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IOrganizationAddressService
            where TContext : DbContext, ICrmOrganizationContext
        {
            services.AddGearTransient<IOrganizationAddressService, TService>();
            services.AddTransient<ICrmOrganizationContext, TContext>();
            return services;
        }

    }
}
