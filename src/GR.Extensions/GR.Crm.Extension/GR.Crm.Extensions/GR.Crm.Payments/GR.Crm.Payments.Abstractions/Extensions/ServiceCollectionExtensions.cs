using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Payments.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Crm Payment module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmPaymentModule<TService, TCService ,TContext>(this IServiceCollection services)
            where TService : class, IPaymentService
            where TCService : class, IPaymentCodeService
            where TContext : DbContext, IPaymentContext
        {
            services.AddGearTransient<IPaymentService, TService>();
            services.AddGearTransient<IPaymentCodeService, TCService>();
            services.AddTransient<IPaymentContext, TContext>();
            return services;
        }
    }
}
