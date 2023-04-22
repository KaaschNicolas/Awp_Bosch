﻿// <auto-generated />
using System;
using Backend.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(BoschContext))]
    [Migration("20230422134202_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.Anmerkung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Anmerkung")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.Property<int>("VermerktVonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Anmerkung")
                        .IsUnique();

                    b.HasIndex("VermerktVonId");

                    b.ToTable("Anmerkungen");
                });

            modelBuilder.Entity("Backend.Models.Diagnose", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.HasKey("Id");

                    b.ToTable("Diagnosen");
                });

            modelBuilder.Entity("Backend.Models.Fehlertyp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fehlerbeschreibung")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.HasKey("Id");

                    b.ToTable("Fehlertypen");
                });

            modelBuilder.Entity("Backend.Models.Geraet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Geraete");
                });

            modelBuilder.Entity("Backend.Models.LagerOrt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LagerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("VerweildauerGelb")
                        .HasColumnType("int");

                    b.Property<int>("VerweildauerRot")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LagerOrte");
                });

            modelBuilder.Entity("Backend.Models.Leiterplatte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Abgeschlossen")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EinschraenkungId")
                        .HasColumnType("int");

                    b.Property<int>("EnddiagnoseId")
                        .HasColumnType("int");

                    b.Property<string>("Fehlerbeschreibung")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)");

                    b.Property<string>("LeiterplattentypId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SerienNummer")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("EinschraenkungId");

                    b.HasIndex("EnddiagnoseId");

                    b.HasIndex("LeiterplattentypId");

                    b.ToTable("Leiterplatten");
                });

            modelBuilder.Entity("Backend.Models.Leiterplattentyp", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LpSachnummer")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("MaxWeitergaben")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Leiterplattentypen");
                });

            modelBuilder.Entity("Backend.Models.Nutzer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdUsername")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Nutzende");
                });

            modelBuilder.Entity("Backend.Models.Umbuchung", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Anmerkung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeiterplatteId")
                        .HasColumnType("int");

                    b.Property<int>("NachId")
                        .HasColumnType("int");

                    b.Property<int>("VerbuchtVonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeiterplatteId");

                    b.HasIndex("NachId");

                    b.HasIndex("VerbuchtVonId");

                    b.ToTable("Umbuchungen");
                });

            modelBuilder.Entity("FehlertypLeiterplatte", b =>
                {
                    b.Property<int>("FehlertypenId")
                        .HasColumnType("int");

                    b.Property<int>("LeiterplattenId")
                        .HasColumnType("int");

                    b.HasKey("FehlertypenId", "LeiterplattenId");

                    b.HasIndex("LeiterplattenId");

                    b.ToTable("FehlertypLeiterplatte");
                });

            modelBuilder.Entity("Backend.Models.Anmerkung", b =>
                {
                    b.HasOne("Backend.Models.Leiterplatte", "Leiterplatte")
                        .WithOne("Anmerkung")
                        .HasForeignKey("Backend.Models.Anmerkung", "Anmerkung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Nutzer", "VermerktVon")
                        .WithMany("Anmerkungen")
                        .HasForeignKey("VermerktVonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Leiterplatte");

                    b.Navigation("VermerktVon");
                });

            modelBuilder.Entity("Backend.Models.Leiterplatte", b =>
                {
                    b.HasOne("Backend.Models.Geraet", "Einschraenkung")
                        .WithMany("Leiterplatten")
                        .HasForeignKey("EinschraenkungId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Diagnose", "Enddiagnose")
                        .WithMany("Leiterplatten")
                        .HasForeignKey("EnddiagnoseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Leiterplattentyp", "Leiterplattentyp")
                        .WithMany("Leiterplatten")
                        .HasForeignKey("LeiterplattentypId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Einschraenkung");

                    b.Navigation("Enddiagnose");

                    b.Navigation("Leiterplattentyp");
                });

            modelBuilder.Entity("Backend.Models.Umbuchung", b =>
                {
                    b.HasOne("Backend.Models.Leiterplatte", "Leiterplatte")
                        .WithMany("Weitergaben")
                        .HasForeignKey("LeiterplatteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.LagerOrt", "Nach")
                        .WithMany("Umbuchungen")
                        .HasForeignKey("NachId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Nutzer", "VerbuchtVon")
                        .WithMany("Umbuchungen")
                        .HasForeignKey("VerbuchtVonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Leiterplatte");

                    b.Navigation("Nach");

                    b.Navigation("VerbuchtVon");
                });

            modelBuilder.Entity("FehlertypLeiterplatte", b =>
                {
                    b.HasOne("Backend.Models.Fehlertyp", null)
                        .WithMany()
                        .HasForeignKey("FehlertypenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Leiterplatte", null)
                        .WithMany()
                        .HasForeignKey("LeiterplattenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Models.Diagnose", b =>
                {
                    b.Navigation("Leiterplatten");
                });

            modelBuilder.Entity("Backend.Models.Geraet", b =>
                {
                    b.Navigation("Leiterplatten");
                });

            modelBuilder.Entity("Backend.Models.LagerOrt", b =>
                {
                    b.Navigation("Umbuchungen");
                });

            modelBuilder.Entity("Backend.Models.Leiterplatte", b =>
                {
                    b.Navigation("Anmerkung")
                        .IsRequired();

                    b.Navigation("Weitergaben");
                });

            modelBuilder.Entity("Backend.Models.Leiterplattentyp", b =>
                {
                    b.Navigation("Leiterplatten");
                });

            modelBuilder.Entity("Backend.Models.Nutzer", b =>
                {
                    b.Navigation("Anmerkungen");

                    b.Navigation("Umbuchungen");
                });
#pragma warning restore 612, 618
        }
    }
}
