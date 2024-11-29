using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortTermStayAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedQueryStringsr2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Listings_ListingId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ListingId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "GuestNames",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestNames",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ListingId",
                table: "Bookings",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Listings_ListingId",
                table: "Bookings",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
