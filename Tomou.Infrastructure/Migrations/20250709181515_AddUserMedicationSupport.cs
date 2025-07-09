using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMedicationSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Dependents_DependentId",
                table: "Medications");

            migrationBuilder.AlterColumn<long>(
                name: "DependentId",
                table: "Medications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Medications",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_UserId",
                table: "Medications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Dependents_DependentId",
                table: "Medications",
                column: "DependentId",
                principalTable: "Dependents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Users_UserId",
                table: "Medications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Dependents_DependentId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Users_UserId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_UserId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Medications");

            migrationBuilder.AlterColumn<long>(
                name: "DependentId",
                table: "Medications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Dependents_DependentId",
                table: "Medications",
                column: "DependentId",
                principalTable: "Dependents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
