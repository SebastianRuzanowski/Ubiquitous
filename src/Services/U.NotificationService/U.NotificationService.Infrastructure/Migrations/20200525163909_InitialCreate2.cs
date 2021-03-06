﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace U.NotificationService.Infrastructure.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IntegrationEventId = table.Column<Guid>(nullable: false),
                    IntegrationEvent = table.Column<string>(nullable: false),
                    IntegrationEventType = table.Column<int>(nullable: false),
                    TimesSent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBasedEventImportancy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Importancy = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    NotificationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBasedEventImportancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBasedEventImportancy_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "Notifications",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification_confirmation",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    User = table.Column<Guid>(nullable: false),
                    NotificationId = table.Column<Guid>(nullable: false),
                    ConfirmationDate = table.Column<DateTime>(nullable: false),
                    ConfirmationType = table.Column<int>(nullable: false),
                    DeviceReceivedId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification_confirmation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_confirmation_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "Notifications",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBasedEventImportancy_NotificationId",
                table: "UserBasedEventImportancy",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_confirmation_NotificationId",
                schema: "Notifications",
                table: "Notification_confirmation",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_confirmation_User",
                schema: "Notifications",
                table: "Notification_confirmation",
                column: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBasedEventImportancy");

            migrationBuilder.DropTable(
                name: "Notification_confirmation",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Notifications");
        }
    }
}
