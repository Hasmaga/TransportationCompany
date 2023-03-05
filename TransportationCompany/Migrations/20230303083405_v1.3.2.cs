using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationCompany.Migrations
{
    /// <inheritdoc />
    public partial class v132 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthType",
                schema: "dbo",
                table: "PassengerLogin",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthType",
                schema: "dbo",
                table: "PassengerLogin");
        }
    }
}
