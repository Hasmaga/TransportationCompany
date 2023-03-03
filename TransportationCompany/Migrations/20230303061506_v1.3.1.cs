using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationCompany.Migrations
{
    /// <inheritdoc />
    public partial class v131 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "Trip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "Trip",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
