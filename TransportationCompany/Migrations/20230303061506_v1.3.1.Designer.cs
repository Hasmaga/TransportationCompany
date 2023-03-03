﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransportationCompany.DbContexts;

#nullable disable

namespace TransportationCompany.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230303061506_v1.3.1")]
    partial class v131
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TransportationCompany.Model.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("BookingDate");

                    b.Property<Guid>("PassengerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Seat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Seat");

                    b.Property<bool>("Status")
                        .HasColumnType("bit")
                        .HasColumnName("Status");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PassengerId");

                    b.HasIndex("TripId");

                    b.ToTable("Booking", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Address");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.HasKey("Id");

                    b.ToTable("Company", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Location", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Passenger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Address");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Avatar");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedDate");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2")
                        .HasColumnName("DoB");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Phone");

                    b.HasKey("Id");

                    b.ToTable("Passenger", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.PassengerComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Comment");

                    b.Property<Guid>("PassengerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PassengerId");

                    b.ToTable("PassengerComment", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.PassengerLogin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<Guid>("PassengerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PasswordHash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PasswordSalt");

                    b.Property<bool>("Status")
                        .HasColumnType("bit")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("PassengerId");

                    b.ToTable("PassengerLogin", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.RouteTrip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FromLocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Price");

                    b.Property<Guid>("ToLocationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("FromLocationId");

                    b.HasIndex("ToLocationId");

                    b.ToTable("RouteTrip", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("ArrivalTime");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("DepartureTime");

                    b.Property<Guid>("RouteTripId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RouteTripId");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("RouteTripId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Trip", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("Capacity");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("RegistrationNumber");

                    b.Property<Guid>("RouteTripId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("RouteTripId");

                    b.ToTable("Vehicle", "dbo");
                });

            modelBuilder.Entity("TransportationCompany.Model.Booking", b =>
                {
                    b.HasOne("TransportationCompany.Model.Passenger", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportationCompany.Model.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("TransportationCompany.Model.PassengerComment", b =>
                {
                    b.HasOne("TransportationCompany.Model.Passenger", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("TransportationCompany.Model.PassengerLogin", b =>
                {
                    b.HasOne("TransportationCompany.Model.Passenger", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("TransportationCompany.Model.RouteTrip", b =>
                {
                    b.HasOne("TransportationCompany.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportationCompany.Model.Location", "From")
                        .WithMany("FromLocation")
                        .HasForeignKey("FromLocationId")
                        .IsRequired();

                    b.HasOne("TransportationCompany.Model.Location", "To")
                        .WithMany("ToLocation")
                        .HasForeignKey("ToLocationId")
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("From");

                    b.Navigation("To");
                });

            modelBuilder.Entity("TransportationCompany.Model.Trip", b =>
                {
                    b.HasOne("TransportationCompany.Model.RouteTrip", "RouteTrip")
                        .WithMany()
                        .HasForeignKey("RouteTripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportationCompany.Model.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RouteTrip");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("TransportationCompany.Model.Vehicle", b =>
                {
                    b.HasOne("TransportationCompany.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransportationCompany.Model.RouteTrip", "RouteTrip")
                        .WithMany()
                        .HasForeignKey("RouteTripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("RouteTrip");
                });

            modelBuilder.Entity("TransportationCompany.Model.Location", b =>
                {
                    b.Navigation("FromLocation");

                    b.Navigation("ToLocation");
                });
#pragma warning restore 612, 618
        }
    }
}
