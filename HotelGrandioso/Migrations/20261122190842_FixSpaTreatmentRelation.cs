using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelGrandioso.Migrations
{
    /// <inheritdoc />
    public partial class FixSpaTreatmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaBooking_Guests_TreatmentId",
                table: "SpaBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_SpaBooking_Treatment_TreatmentId1",
                table: "SpaBooking");

            migrationBuilder.DropIndex(
                name: "IX_SpaBooking_TreatmentId1",
                table: "SpaBooking");

            migrationBuilder.DropColumn(
                name: "TreatmentId1",
                table: "SpaBooking");

            migrationBuilder.CreateIndex(
                name: "IX_SpaBooking_GuestId",
                table: "SpaBooking",
                column: "GuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpaBooking_Guests_GuestId",
                table: "SpaBooking",
                column: "GuestId",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaBooking_Treatment_TreatmentId",
                table: "SpaBooking",
                column: "TreatmentId",
                principalTable: "Treatment",
                principalColumn: "TreatmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaBooking_Guests_GuestId",
                table: "SpaBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_SpaBooking_Treatment_TreatmentId",
                table: "SpaBooking");

            migrationBuilder.DropIndex(
                name: "IX_SpaBooking_GuestId",
                table: "SpaBooking");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId1",
                table: "SpaBooking",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpaBooking_TreatmentId1",
                table: "SpaBooking",
                column: "TreatmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SpaBooking_Guests_TreatmentId",
                table: "SpaBooking",
                column: "TreatmentId",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaBooking_Treatment_TreatmentId1",
                table: "SpaBooking",
                column: "TreatmentId1",
                principalTable: "Treatment",
                principalColumn: "TreatmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
