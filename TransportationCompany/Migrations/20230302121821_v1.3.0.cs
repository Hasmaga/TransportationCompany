using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationCompany.Migrations
{
    /// <inheritdoc />
    public partial class v130 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Passenger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                schema: "dbo",
                table: "Passenger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "Passenger",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DoB",
                schema: "dbo",
                table: "Passenger",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "dbo",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "dbo",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "dbo",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "DoB",
                schema: "dbo",
                table: "Passenger");
        }
    }
}
