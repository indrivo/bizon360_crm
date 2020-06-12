﻿// <auto-generated />
using System;
using GR.Crm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GR.Crm.Migrations
{
    [DbContext(typeof(CrmDbContext))]
    [Migration("20200206084750_CrmDbContextt_addOrganizationDescription")]
    partial class CrmDbContextt_addOrganizationDescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Crm")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAudit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DatabaseContextName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("RecordId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("TrackEventType");

                    b.Property<string>("TypeFullName");

                    b.Property<string>("UserName");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("TrackAudits");
                });

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAuditDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("PropertyName");

                    b.Property<string>("PropertyType");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("TrackAuditId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TrackAuditId");

                    b.ToTable("TrackAuditDetails");
                });

            modelBuilder.Entity("GR.Crm.Abstractions.Models.Currency", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DecimalDigits");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NativeSymbol")
                        .IsRequired();

                    b.Property<string>("PluralName");

                    b.Property<decimal>("Rounding");

                    b.Property<string>("Symbol")
                        .IsRequired();

                    b.HasKey("Code");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("GR.Crm.Abstractions.Models.JobPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("JobPositions");
                });

            modelBuilder.Entity("GR.Crm.Contracts.Abstractions.Models.ContractSection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<Guid>("ContractTemplateId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("TemplateValue");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("ContractTemplateId");

                    b.ToTable("ContractSections");
                });

            modelBuilder.Entity("GR.Crm.Contracts.Abstractions.Models.ContractTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("ContractTemplates");
                });

            modelBuilder.Entity("GR.Crm.Leads.Abstractions.Models.Lead", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime?>("ClarificationDeadLine");

                    b.Property<DateTime>("Created");

                    b.Property<string>("CurrencyCode");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("LeadStateId");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime?>("OfferDeadLine");

                    b.Property<Guid>("OrganizationId");

                    b.Property<Guid>("PipeLineId");

                    b.Property<Guid>("StageId");

                    b.Property<DateTime>("StartDate");

                    b.Property<Guid?>("TeamId");

                    b.Property<Guid?>("TenantId");

                    b.Property<decimal>("Value");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyCode");

                    b.HasIndex("LeadStateId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("PipeLineId");

                    b.HasIndex("StageId");

                    b.HasIndex("TeamId");

                    b.ToTable("Leads");
                });

            modelBuilder.Entity("GR.Crm.Leads.Abstractions.Models.LeadState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("JobPositionId");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("OrganizationId");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("JobPositionId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.ContactWebProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<Guid>("ContactId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Version");

                    b.Property<Guid>("WebProfileId");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("WebProfileId");

                    b.ToTable("ContactWebProfiles");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Interval")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Industry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Industries");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Bank");

                    b.Property<DateTime>("Changed");

                    b.Property<Guid?>("CityId");

                    b.Property<int>("ClientType");

                    b.Property<Guid?>("CountryId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("EmployeeId");

                    b.Property<string>("FiscalCode")
                        .HasMaxLength(128);

                    b.Property<string>("IBANCode")
                        .HasMaxLength(128);

                    b.Property<Guid?>("IndustryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Street")
                        .HasMaxLength(128);

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<string>("WebSite")
                        .HasMaxLength(50);

                    b.Property<string>("Zip")
                        .HasMaxLength(28);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("IndustryId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.WebProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<byte[]>("Icon");

                    b.Property<string>("IconName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("ProviderName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Url")
                        .HasMaxLength(50);

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("WebProfiles");
                });

            modelBuilder.Entity("GR.Crm.PipeLines.Abstractions.Models.PipeLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid?>("WorkFlowId");

                    b.HasKey("Id");

                    b.ToTable("PipeLines");
                });

            modelBuilder.Entity("GR.Crm.PipeLines.Abstractions.Models.Stage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("PipeLineId");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.Property<Guid?>("WorkFlowStateId");

                    b.HasKey("Id");

                    b.HasIndex("PipeLineId");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("GR.Crm.Teams.Abstractions.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("GR.Crm.Teams.Abstractions.Models.TeamMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<Guid>("TeamId");

                    b.Property<Guid>("TeamRoleId");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("TeamRoleId");

                    b.ToTable("TeamMembers");
                });

            modelBuilder.Entity("GR.Crm.Teams.Abstractions.Models.TeamRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("TeamRoles");
                });

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("GR.Audit.Abstractions.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Crm.Contracts.Abstractions.Models.ContractSection", b =>
                {
                    b.HasOne("GR.Crm.Contracts.Abstractions.Models.ContractTemplate", "ContractTemplate")
                        .WithMany("Sections")
                        .HasForeignKey("ContractTemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Crm.Leads.Abstractions.Models.Lead", b =>
                {
                    b.HasOne("GR.Crm.Abstractions.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyCode");

                    b.HasOne("GR.Crm.Leads.Abstractions.Models.LeadState", "LeadState")
                        .WithMany()
                        .HasForeignKey("LeadStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.PipeLines.Abstractions.Models.PipeLine", "PipeLine")
                        .WithMany()
                        .HasForeignKey("PipeLineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.PipeLines.Abstractions.Models.Stage", "Stage")
                        .WithMany()
                        .HasForeignKey("StageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.Teams.Abstractions.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Contact", b =>
                {
                    b.HasOne("GR.Crm.Abstractions.Models.JobPosition", "JobPosition")
                        .WithMany()
                        .HasForeignKey("JobPositionId");

                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.Organization", "Organization")
                        .WithMany("Contacts")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.ContactWebProfile", b =>
                {
                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.Contact", "Contact")
                        .WithMany("ContactWebProfiles")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.WebProfile", "WebProfile")
                        .WithMany()
                        .HasForeignKey("WebProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Crm.Organizations.Abstractions.Models.Organization", b =>
                {
                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("GR.Crm.Organizations.Abstractions.Models.Industry", "Industry")
                        .WithMany()
                        .HasForeignKey("IndustryId");
                });

            modelBuilder.Entity("GR.Crm.PipeLines.Abstractions.Models.Stage", b =>
                {
                    b.HasOne("GR.Crm.PipeLines.Abstractions.Models.PipeLine", "PipeLine")
                        .WithMany("Stages")
                        .HasForeignKey("PipeLineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.Crm.Teams.Abstractions.Models.TeamMember", b =>
                {
                    b.HasOne("GR.Crm.Teams.Abstractions.Models.Team", "Team")
                        .WithMany("TeamMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GR.Crm.Teams.Abstractions.Models.TeamRole", "TeamRole")
                        .WithMany()
                        .HasForeignKey("TeamRoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}