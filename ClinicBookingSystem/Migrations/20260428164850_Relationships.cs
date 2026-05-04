using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_timeSlots_doctorId",
                table: "timeSlots",
                column: "doctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_userId",
                table: "doctors",
                column: "userId",
                principalTable: "users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_timeSlots_doctors_doctorId",
                table: "timeSlots",
                column: "doctorId",
                principalTable: "doctors",
                principalColumn: "doctorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_userId",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_timeSlots_doctors_doctorId",
                table: "timeSlots");

            migrationBuilder.DropIndex(
                name: "IX_timeSlots_doctorId",
                table: "timeSlots");
        }
    }
}
