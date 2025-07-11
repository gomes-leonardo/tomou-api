using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSnoozeAndStatusToMedicationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToTake",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "WasTaken",
                table: "MedicationLogs");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "MedicationLogs",
                newName: "ScheduledFor");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "Medications",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Medications",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Medications",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "TimesToTake",
                table: "Medications",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "SnoozedUntil",
                table: "MedicationLogs",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicationLogs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "TakenAt",
                table: "MedicationLogs",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "TimesToTake",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "SnoozedUntil",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicationLogs");

            migrationBuilder.DropColumn(
                name: "TakenAt",
                table: "MedicationLogs");

            migrationBuilder.RenameColumn(
                name: "ScheduledFor",
                table: "MedicationLogs",
                newName: "Date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeToTake",
                table: "Medications",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "WasTaken",
                table: "MedicationLogs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
