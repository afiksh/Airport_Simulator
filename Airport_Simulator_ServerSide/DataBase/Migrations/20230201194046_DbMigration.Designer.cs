﻿// <auto-generated />
using System;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataBase.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230201194046_DbMigration")]
    partial class DbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AirportSimulator_FinalProject_Models.Models.Airplane", b =>
                {
                    b.Property<int>("AirplaneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AirplaneId"), 1L, 1);

                    b.Property<int>("CompanyName")
                        .HasColumnType("int");

                    b.Property<int>("CrewMembers")
                        .HasColumnType("int");

                    b.Property<int>("CurrentFuelState")
                        .HasColumnType("int");

                    b.Property<int>("CurrentWeight")
                        .HasColumnType("int");

                    b.Property<int>("Destenation")
                        .HasColumnType("int");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<bool?>("IsLanding")
                        .HasColumnType("bit");

                    b.Property<int>("LegId")
                        .HasColumnType("int");

                    b.Property<int>("MaxPassengers")
                        .HasColumnType("int");

                    b.Property<int>("MaxSpeed")
                        .HasColumnType("int");

                    b.Property<int>("MaxWeight")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Origin")
                        .HasColumnType("int");

                    b.Property<Guid>("SerialNumber")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Width")
                        .HasColumnType("float");

                    b.HasKey("AirplaneId");

                    b.HasIndex("LegId");

                    b.ToTable("Airplanes");
                });

            modelBuilder.Entity("AirportSimulator_FinalProject_Models.Models.Leg", b =>
                {
                    b.Property<int>("LegId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LegId"), 1L, 1);

                    b.Property<double>("Duration")
                        .HasColumnType("float");

                    b.Property<bool>("IsEmpty")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("LegId");

                    b.ToTable("Legs");
                });

            modelBuilder.Entity("AirportSimulator_FinalProject_Models.Models.Logger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AirplaneId")
                        .HasColumnType("int");

                    b.Property<DateTime>("In")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LegId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Out")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AirplaneId");

                    b.HasIndex("LegId");

                    b.ToTable("Loggers");
                });

            modelBuilder.Entity("AirportSimulator_FinalProject_Models.Models.Airplane", b =>
                {
                    b.HasOne("AirportSimulator_FinalProject_Models.Models.Leg", "CurrentLeg")
                        .WithMany()
                        .HasForeignKey("LegId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentLeg");
                });

            modelBuilder.Entity("AirportSimulator_FinalProject_Models.Models.Logger", b =>
                {
                    b.HasOne("AirportSimulator_FinalProject_Models.Models.Airplane", "Airplane")
                        .WithMany()
                        .HasForeignKey("AirplaneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirportSimulator_FinalProject_Models.Models.Leg", "Leg")
                        .WithMany()
                        .HasForeignKey("LegId");

                    b.Navigation("Airplane");

                    b.Navigation("Leg");
                });
#pragma warning restore 612, 618
        }
    }
}
