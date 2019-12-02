﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using U.SubscriptionService.Persistance.Contexts;

namespace U.SubscriptionService.Persistance.Migrations
{
    [DbContext(typeof(SubscriptionContext))]
    [Migration("20191130190208_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("U.SubscriptionService.Domain.AllowedEvents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Allowed");

                    b.Property<Guid?>("UserSubscriptionId");

                    b.HasKey("Id");

                    b.HasIndex("UserSubscriptionId");

                    b.ToTable("AllowedEvents");
                });

            modelBuilder.Entity("U.SubscriptionService.Domain.SignalRConnection", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("ConnectionId");

                    b.Property<Guid?>("UserSubscriptionId");

                    b.HasKey("UserId", "ConnectionId");

                    b.HasIndex("UserSubscriptionId");

                    b.ToTable("SignalRConnections","Subscriptions");
                });

            modelBuilder.Entity("U.SubscriptionService.Domain.UserSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserSubscription","Subscriptions");
                });

            modelBuilder.Entity("U.SubscriptionService.Domain.AllowedEvents", b =>
                {
                    b.HasOne("U.SubscriptionService.Domain.UserSubscription")
                        .WithMany("AllowedEvents")
                        .HasForeignKey("UserSubscriptionId");
                });

            modelBuilder.Entity("U.SubscriptionService.Domain.SignalRConnection", b =>
                {
                    b.HasOne("U.SubscriptionService.Domain.UserSubscription")
                        .WithMany("Connections")
                        .HasForeignKey("UserSubscriptionId");
                });

            modelBuilder.Entity("U.SubscriptionService.Domain.UserSubscription", b =>
                {
                    b.OwnsOne("U.SubscriptionService.Domain.Preferences", "Preferences", b1 =>
                        {
                            b1.Property<Guid>("UserSubscriptionId");

                            b1.Property<bool>("DoNotNotifyAnyoneAboutMyActivity");

                            b1.Property<int>("MinimalImportancyLevel");

                            b1.Property<int>("NumberOfWelcomeMessages");

                            b1.Property<bool>("OrderByCreationTimeDescending");

                            b1.Property<bool>("OrderByImportancyDescending");

                            b1.Property<bool>("SeeReadNotifications");

                            b1.Property<bool>("SeeUnreadNotifications");

                            b1.HasKey("UserSubscriptionId");

                            b1.ToTable("UserSubscription","Subscriptions");

                            b1.HasOne("U.SubscriptionService.Domain.UserSubscription")
                                .WithOne("Preferences")
                                .HasForeignKey("U.SubscriptionService.Domain.Preferences", "UserSubscriptionId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
