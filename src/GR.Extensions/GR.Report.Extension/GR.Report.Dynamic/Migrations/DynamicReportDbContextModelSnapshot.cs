﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using GR.Report.Dynamic.Data;

namespace GR.Report.Dynamic.Migrations
{
    [DbContext(typeof(DynamicReportDbContext))]
    partial class DynamicReportDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Report")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GR.Report.Abstractions.Models.DynamicReportDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<Guid>("DynamicReportFolderId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<string>("ReportData");

                    b.Property<Guid?>("TenantId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("DynamicReportFolderId");

                    b.ToTable("DynamicReports");
                });

            modelBuilder.Entity("GR.Report.Abstractions.Models.DynamicReportFolder", b =>
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

                    b.ToTable("DynamicReportsFolders");
                });

            modelBuilder.Entity("GR.Report.Abstractions.Models.DynamicReportDbModel", b =>
                {
                    b.HasOne("GR.Report.Abstractions.Models.DynamicReportFolder", "DynamicReportFolder")
                        .WithMany("Reports")
                        .HasForeignKey("DynamicReportFolderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
