﻿// <auto-generated />
using System;
using App.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Core.Migrations
{
    [DbContext(typeof(BoschContext))]
    [Migration("20230428085623_PcbTypAddedDescription")]
    partial class PcbTypAddedDescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("App.Core.Models.AuditEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Message")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("AuditEntries");
                });

            modelBuilder.Entity("App.Core.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.Property<int>("NotedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotedById");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("App.Core.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("App.Core.Models.Diagnose", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.HasKey("Id");

                    b.ToTable("Diagnoses");
                });

            modelBuilder.Entity("App.Core.Models.ErrorType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ErrorDescribtion")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.HasKey("Id");

                    b.ToTable("ErrorTypes");
                });

            modelBuilder.Entity("App.Core.Models.Pcb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CommentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DiagnoseId")
                        .HasColumnType("int");

                    b.Property<string>("ErrorDescription")
                        .HasColumnType("nvarchar(650)");

                    b.Property<bool>("Finalized")
                        .HasColumnType("bit");

                    b.Property<int>("PcbTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("RestrictionId")
                        .HasColumnType("int");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CommentId")
                        .IsUnique()
                        .HasFilter("[CommentId] IS NOT NULL");

                    b.HasIndex("DiagnoseId");

                    b.HasIndex("PcbTypeId");

                    b.HasIndex("RestrictionId");

                    b.ToTable("Pcbs");
                });

            modelBuilder.Entity("App.Core.Models.PcbType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxTransfer")
                        .HasColumnType("int");

                    b.Property<string>("PcbPartNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("PcbTypes");
                });

            modelBuilder.Entity("App.Core.Models.StorageLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DwellTimeRed")
                        .HasColumnType("int");

                    b.Property<int>("DwellTimeYellow")
                        .HasColumnType("int");

                    b.Property<string>("StorageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("StorageLocations");
                });

            modelBuilder.Entity("App.Core.Models.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NotedById")
                        .HasColumnType("int");

                    b.Property<int?>("PcbId")
                        .HasColumnType("int");

                    b.Property<int>("StorageLocationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NotedById");

                    b.HasIndex("PcbId");

                    b.HasIndex("StorageLocationId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("App.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AdUsername")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ErrorTypePcb", b =>
                {
                    b.Property<int>("ErrorTypesId")
                        .HasColumnType("int");

                    b.Property<int>("PcbsId")
                        .HasColumnType("int");

                    b.HasKey("ErrorTypesId", "PcbsId");

                    b.HasIndex("PcbsId");

                    b.ToTable("ErrorTypePcb");
                });

            modelBuilder.Entity("App.Core.Models.Comment", b =>
                {
                    b.HasOne("App.Core.Models.User", "NotedBy")
                        .WithMany("Comments")
                        .HasForeignKey("NotedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotedBy");
                });

            modelBuilder.Entity("App.Core.Models.Pcb", b =>
                {
                    b.HasOne("App.Core.Models.Comment", "Comment")
                        .WithOne("Pcb")
                        .HasForeignKey("App.Core.Models.Pcb", "CommentId");

                    b.HasOne("App.Core.Models.Diagnose", "Diagnose")
                        .WithMany("Pcbs")
                        .HasForeignKey("DiagnoseId");

                    b.HasOne("App.Core.Models.PcbType", "PcbType")
                        .WithMany("Pcbs")
                        .HasForeignKey("PcbTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Core.Models.Device", "Restriction")
                        .WithMany("Pcbs")
                        .HasForeignKey("RestrictionId");

                    b.Navigation("Comment");

                    b.Navigation("Diagnose");

                    b.Navigation("PcbType");

                    b.Navigation("Restriction");
                });

            modelBuilder.Entity("App.Core.Models.Transfer", b =>
                {
                    b.HasOne("App.Core.Models.User", "NotedBy")
                        .WithMany("Transfers")
                        .HasForeignKey("NotedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Core.Models.Pcb", "Pcb")
                        .WithMany("Transfers")
                        .HasForeignKey("PcbId");

                    b.HasOne("App.Core.Models.StorageLocation", "StorageLocation")
                        .WithMany("Transfers")
                        .HasForeignKey("StorageLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotedBy");

                    b.Navigation("Pcb");

                    b.Navigation("StorageLocation");
                });

            modelBuilder.Entity("ErrorTypePcb", b =>
                {
                    b.HasOne("App.Core.Models.ErrorType", null)
                        .WithMany()
                        .HasForeignKey("ErrorTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Core.Models.Pcb", null)
                        .WithMany()
                        .HasForeignKey("PcbsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("App.Core.Models.Comment", b =>
                {
                    b.Navigation("Pcb");
                });

            modelBuilder.Entity("App.Core.Models.Device", b =>
                {
                    b.Navigation("Pcbs");
                });

            modelBuilder.Entity("App.Core.Models.Diagnose", b =>
                {
                    b.Navigation("Pcbs");
                });

            modelBuilder.Entity("App.Core.Models.Pcb", b =>
                {
                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("App.Core.Models.PcbType", b =>
                {
                    b.Navigation("Pcbs");
                });

            modelBuilder.Entity("App.Core.Models.StorageLocation", b =>
                {
                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("App.Core.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}
