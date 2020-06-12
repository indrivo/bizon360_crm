using System;
using System.Linq;
using GR.Core.Extensions;
using GR.Crm.Leads.Abstractions.Helpers;
using GR.Crm.Leads.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Leads.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add lead module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TLead"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmLeadModule<TService, TLeadNotificationService, TContext, TLead>(this IServiceCollection services)
            where TService : class, ILeadService<TLead> where TLeadNotificationService : class, ILeadNotificationService
            where TContext : DbContext, ILeadContext<TLead>
            where TLead : Lead
        {
            services.AddGearTransient<ILeadService<TLead>, TService>();
            services.AddGearTransient<ILeadNotificationService, TLeadNotificationService>();
            services.AddTransient<ILeadContext<TLead>, TContext>();
            return services;
        }


       /// <summary>
       /// Add lead product
       /// </summary>
       /// <typeparam name="TService"></typeparam>
       /// <typeparam name="TContext"></typeparam>
       /// <param name="services"></param>
       /// <returns></returns>
        public static IServiceCollection AddCrmLeadProductModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IProductService
            where TContext : DbContext, ILeadContext<Lead>
        {
            services.AddGearTransient<IProductService, TService>();
            services.AddTransient<ILeadContext<Lead>, TContext>();
            return services;
        }

       /// <summary>
       /// Add lead agreement
       /// </summary>
       /// <typeparam name="TService"></typeparam>
       /// <typeparam name="TContext"></typeparam>
       /// <param name="services"></param>
       /// <returns></returns>
       public static IServiceCollection AddCrmLeadAgreementModule<TService, TContext>(this IServiceCollection services)
           where TService : class, IAgreementService
           where TContext : DbContext, ILeadContext<Lead>
       {
           services.AddGearTransient<IAgreementService, TService>();
           services.AddTransient<ILeadContext<Lead>, TContext>();
           return services;
       }


        /// <summary>
        /// Add tags
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomTags(this IServiceCollection services)
        {
            // system
            ContractTagsHelper.AddNewKey("CurrentDate", (agreement, objects) => DateTime.Now.ToString("dd/MM/yyyy"));

            // Lead organization
            ContractTagsHelper.AddNewKey("OrganizationName", (agreement, objects) => agreement.Organization?.Name ?? "");
            ContractTagsHelper.AddNewKey("IDNO", (agreement, objects) => agreement.Organization?.FiscalCode ?? "");
            ContractTagsHelper.AddNewKey("IBAN", (agreement, objects) => agreement.Organization?.IBANCode ?? "");
            ContractTagsHelper.AddNewKey("Bank", (agreement, objects) => agreement.Organization?.Bank ?? "");
            ContractTagsHelper.AddNewKey("Phone", (agreement, objects) => agreement.Organization?.Phone ?? "");
            ContractTagsHelper.AddNewKey("Email", (agreement, objects) => agreement.Organization?.Email ?? "");
            ContractTagsHelper.AddNewKey("CodTva", (agreement, objects) => agreement.Organization?.VitCode ?? "");
            ContractTagsHelper.AddNewKey("CodSwift", (agreement, objects) => agreement.Organization?.CodSwift ?? "");
           
            // Address
            ContractTagsHelper.AddNewKey("Region", (agreement, objects) =>
            {
                var address = agreement.OrganizationAddress;
                if (address == null) return "";
                return address.City?.Region?.Name ?? "";
            });
            ContractTagsHelper.AddNewKey("City", (agreement, objects) =>
            {
                var address = agreement.OrganizationAddress;
                if (address == null) return "";
                return address.City?.Name ?? "";
            });
            ContractTagsHelper.AddNewKey("Street", (agreement, objects) =>
            {
                var address = agreement.OrganizationAddress;
                if (address == null) return "";
                return address.Street ?? "";
            });
           
            //Contact
            ContractTagsHelper.AddNewKey("Contact", (agreement, objects) =>
            {
                var contact = agreement.Contact;
                if (contact == null) return "";
                return contact.FirstName ?? "" + " " + contact.LastName;
            });

            // Lead
           
            ContractTagsHelper.AddNewKey("Currency", (agreement, objects) => agreement.Lead.Value.ToString());

            // Agreement
            ContractTagsHelper.AddNewKey("Value", (agreement, objects) => agreement.Values.ToString());
            ContractTagsHelper.AddNewKey("Commission", (agreement, objects) => agreement.Commission.ToString());


            return services;
        }
    }
}
