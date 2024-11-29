using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortTermStayAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedQueryStringsr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Listings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
