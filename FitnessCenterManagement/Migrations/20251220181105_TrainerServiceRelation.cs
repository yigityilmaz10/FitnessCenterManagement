using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterManagement.Migrations
{
    /// <inheritdoc />
    public partial class TrainerServiceRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Trainers");

            migrationBuilder.RenameColumn(
                name: "RequestText",
                table: "AIRequestLogs",
                newName: "Goal");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AIRequestLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "AIRequestLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "AIRequestLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "AIRequestLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_ServiceId",
                table: "Trainers",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Services_ServiceId",
                table: "Trainers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Services_ServiceId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_ServiceId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "AIRequestLogs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AIRequestLogs");

            migrationBuilder.RenameColumn(
                name: "Goal",
                table: "AIRequestLogs",
                newName: "RequestText");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AIRequestLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "AIRequestLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
