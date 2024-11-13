﻿// <auto-generated />
using ExoticHistoricSites.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExoticHistoricSites.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241113093003_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.HistoricSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AverageRating")
                        .HasColumnType("TEXT");

                    b.Property<string>("Countries")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("MainImageBase64")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("HistoricSites");
                });

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.HistoricSiteImage", b =>
                {
                    b.Property<int>("HistoricSiteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SiteImageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("HistoricSiteId", "SiteImageId");

                    b.ToTable("HistoricSiteImages");
                });

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.SiteImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageBase64")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SiteImages");
                });

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.SiteRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HistoricSiteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "HistoricSiteId")
                        .IsUnique();

                    b.ToTable("SiteRatings");
                });

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExoticHistoricSites.Shared.Models.UserFavorite", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HistoricSiteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "HistoricSiteId");

                    b.ToTable("UserFavorites");
                });
#pragma warning restore 612, 618
        }
    }
}
