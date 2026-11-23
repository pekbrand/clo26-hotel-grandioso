using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelGrandioso.Migrations
{
    /// <inheritdoc />
    public partial class SeedHotelData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "RoomTypes",
            columns: new[] { "RoomTypeId", "Name", "Description" },
            values: new object[,]
            {
                { 1, "Standard", "Standardrum" },
                { 2, "Luxury", "Lyxsvit" },
                { 3, "Bridal", "Bröllopsvit" }
            });

            migrationBuilder.InsertData(
            table: "Treatment",
            columns: new[] { "TreatmentId", "Name", "DurationMinutes","Price" },
            values: new object[,]
            {
                { 1, "Entré",              60, 150m },
                { 2, "Massage",            60, 950m },
                { 3, "Pedikyr (stortån)",  15, 350m },
                { 4, "Pedikyr (hel fot)",  45, 650m }
            });

            migrationBuilder.InsertData(
            table: "Rooms",
            columns: new[] { "RoomNumber", "Floor", "SizeM2", "RoomTypeId" },
            values: new object[,]
            {
                { 101, 1,  80, 1 },
                { 102, 1,  80, 1 },
                { 103, 1,  80, 1 },
                { 104, 1,  80, 1 },
                { 105, 1,  80, 1 },

                { 201, 2, 100, 1 },
                { 202, 2, 100, 1 },
                { 203, 2, 100, 1 },
                { 204, 2, 100, 1 },
                { 205, 2, 100, 1 },

                { 301, 3, 120, 1 },
                { 302, 3, 120, 1 },
                { 303, 3, 120, 1 },
                { 304, 3, 180, 2 },
                { 305, 3, 220, 3 }
            });
            var priceStart = new DateOnly(2026, 1, 1);
            migrationBuilder.InsertData(
                table: "RoomPrice",
                columns: new[] { "Id", "RoomNumber", "PricePerNight", "FromDate", "ToDate" },
            values: new object[,]
            {
                {  1, 101,  800m, priceStart, null! },
                {  2, 102,  800m, priceStart, null! },
                {  3, 103,  800m, priceStart, null! },
                {  4, 104,  800m, priceStart, null! },
                {  5, 105,  800m, priceStart, null! },

                {  6, 201, 1000m, priceStart, null! },
                {  7, 202, 1000m, priceStart, null! },
                {  8, 203, 1000m, priceStart, null! },
                {  9, 204, 1000m, priceStart, null! },
                { 10, 205, 1000m, priceStart, null! },

                { 11, 301, 1200m, priceStart, null! },
                { 12, 302, 1200m, priceStart, null! },
                { 13, 303, 1200m, priceStart, null! },
                { 14, 304, 2800m, priceStart, null! },
                { 15, 305, 3500m, priceStart, null! }
            });
            }
            

            /// <inheritdoc />
            protected override void Down(MigrationBuilder migrationBuilder)
            {

            }
    }
}
