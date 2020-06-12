using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GR.Audit.Contexts;
using GR.Core;
using GR.Core.Helpers;
using GR.Crm.Contracts.Abstractions;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.CurrencyViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.Teams.Abstractions;
using Microsoft.EntityFrameworkCore;
using GR.Crm.Teams.Abstractions.Models;
using Mapster;

namespace GR.Crm.Data
{
    public class CrmDbContext : TrackerDbContext, ILeadContext<Lead>, IContractsContext, ICrmTeamContext, IPaymentContext
    {
        /// <summary>
        /// Schema
        /// Do not remove this, is used on audit 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public const string Schema = "Crm";

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        #region Entities

        /// <summary>
        /// Organization
        /// </summary>
        public virtual DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public virtual DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        /// <summary>
        /// Cities 
        /// </summary>
        public virtual DbSet<City> Cities { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public virtual DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Countries
        /// </summary>
        public virtual DbSet<CrmCountry> Countries { get; set; }


        /// <summary>
        /// Industries
        /// </summary>
        public virtual DbSet<Industry> Industries { get; set; }

        /// <summary>
        /// Employees
        /// </summary>
        public virtual DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        public virtual DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Contact web profile
        /// </summary>
        public virtual DbSet<ContactWebProfile> ContactWebProfiles { get; set; }

        /// <summary>
        /// web profile
        /// </summary>
        public virtual DbSet<WebProfile> WebProfiles { get; set; }

        /// <summary>
        /// PipeLines
        /// </summary>
        public virtual DbSet<PipeLine> PipeLines { get; set; }

        /// <summary>
        /// Stages
        /// </summary>
        public virtual DbSet<Stage> Stages { get; set; }

        /// <summary>
        /// Contract templates
        /// </summary>
        public virtual DbSet<ContractTemplate> ContractTemplates { get; set; }

        /// <summary>
        /// Contract sections
        /// </summary>
        public virtual DbSet<ContractSection> ContractSections { get; set; }

        /// <summary>
        /// Job positions
        /// </summary>
        public virtual DbSet<JobPosition> JobPositions { get; set; }

        /// <summary>
        /// Currencies
        /// </summary>
        public virtual DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Leads
        /// </summary>
        public virtual DbSet<Lead> Leads { get; set; }

        /// <summary>
        /// Lead states
        /// </summary>
        public virtual DbSet<LeadState> States { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        public virtual DbSet<Agreement> Agreements { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
       public virtual  DbSet<Product> Products { get; set; }

        /// <summary>
        /// Team
        /// </summary>
        public virtual DbSet<Team> Teams { get; set; }

        /// <summary>
        /// Team members
        /// </summary>
        public virtual DbSet<TeamMember> TeamMembers { get; set; }

        /// <summary>
        /// Team role
        /// </summary>
        public virtual DbSet<TeamRole> TeamRoles { get; set; }


        /// <summary>
        /// Payment
        /// </summary>
        public virtual DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Payment Mappers
        /// </summary>
        public virtual DbSet<PaymentMapped> PaymentMappers { get; set; }

        /// <summary>
        /// Payment Codes
        /// </summary>
        public virtual DbSet<PaymentCode> PaymentCodes { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        public virtual DbSet<Source> Sources { get; set; }

        /// <summary>
        /// Solution Types
        /// </summary>
        public virtual DbSet<SolutionType> SolutionTypes { get; set; }

        /// <summary>
        /// Technology Types
        /// </summary>
        public virtual DbSet<TechnologyType> TechnologyTypes { get; set; }

        /// <summary>
        /// Service Types
        /// </summary>
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }

        /// <summary>
        ///  Product Types
        /// </summary>
        public virtual DbSet<ProductType> ProductTypes { get; set; }


        #endregion

        /// <summary>
        /// This method is invoked on system installation
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override Task InvokeSeedAsync(IServiceProvider services)
        {
            GearApplication.BackgroundTaskQueue.PushBackgroundWorkItemInQueue(async x =>
            {
                var dataService = IoC.Resolve<ICrmTeamService>();
                if (dataService == null) throw new Exception("ICrmTeamService is not registered");
                await dataService.SeedTeamRole();
            });

            GearApplication.BackgroundTaskQueue.PushBackgroundWorkItemInQueue(async x =>
            {
                var dataService = IoC.Resolve<ILeadService<Lead>>();
                if (dataService == null) throw new Exception("ILeadService is not registered");
                await dataService.SeedSystemLeadState();
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);

            builder.Entity<Currency>().HasKey(x => x.Code);
            builder.Entity<Organization>().HasMany<Lead>().WithOne(x => x.Organization);
            var currenciesFilePath = Path.Combine(AppContext.BaseDirectory, "Configuration/currencies.json");
            var currencies = JsonParser.ReadObjectDataFromJsonFile<Dictionary<string, CurrencyViewModel>>(currenciesFilePath)
                .Select(x => x.Value.Adapt<Currency>()).ToList();
            builder.Entity<Currency>().HasData(currencies);
        }
    }
}
