﻿// <auto-generated />
using System;
using GR.TaskManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GR.TaskManager.Migrations
{
    [DbContext(typeof(TaskManagerDbContext))]
    partial class TaskManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Task")
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

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Files");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid?>("LeadId");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("TaskNumber")
                        .IsRequired();

                    b.Property<int>("TaskPriority");

                    b.Property<Guid?>("TaskTypeId");

                    b.Property<Guid?>("TenantId");

                    b.Property<Guid>("UserId");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("EndDate");

                    b.HasIndex("TaskTypeId");

                    b.HasIndex("UserId");

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("UserId", "IsDeleted");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.TaskAssignedUser", b =>
                {
                    b.Property<Guid>("TaskId");

                    b.Property<Guid>("UserId");

                    b.HasKey("TaskId", "UserId");

                    b.ToTable("TaskAssignedUsers");
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.TaskItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskItems");
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.TaskType", b =>
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

                    b.ToTable("TaskTypes");
                });

            modelBuilder.Entity("GR.Audit.Abstractions.Models.TrackAuditDetails", b =>
                {
                    b.HasOne("GR.Audit.Abstractions.Models.TrackAudit")
                        .WithMany("AuditDetailses")
                        .HasForeignKey("TrackAuditId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.Task", b =>
                {
                    b.HasOne("GR.TaskManager.Abstractions.Models.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId");
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.TaskAssignedUser", b =>
                {
                    b.HasOne("GR.TaskManager.Abstractions.Models.Task", "Task")
                        .WithMany("AssignedUsers")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GR.TaskManager.Abstractions.Models.TaskItem", b =>
                {
                    b.HasOne("GR.TaskManager.Abstractions.Models.Task", "Task")
                        .WithMany("TaskItems")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
