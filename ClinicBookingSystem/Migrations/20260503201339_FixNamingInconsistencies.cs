using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixNamingInconsistencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_specialities",
                table: "specialities");

            migrationBuilder.RenameTable(
                name: "specialities",
                newName: "specialties");

            migrationBuilder.RenameColumn(
                name: "timeSlotId",
                table: "appointments",
                newName: "timeslotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_specialties",
                table: "specialties",
                column: "specialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_timeslotId",
                table: "appointments",
                column: "timeslotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_timeSlots_timeslotId",
                table: "appointments",
                column: "timeslotId",
                principalTable: "timeSlots",
                principalColumn: "timeslotId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_timeSlots_timeslotId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_timeslotId",
                table: "appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_specialties",
                table: "specialties");

            migrationBuilder.RenameTable(
                name: "specialties",
                newName: "specialities");

            migrationBuilder.RenameColumn(
                name: "timeslotId",
                table: "appointments",
                newName: "timeSlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_specialities",
                table: "specialities",
                column: "specialtyId");
        }
    }
}
