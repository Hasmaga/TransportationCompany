using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationCompany.Migrations
{
    /// <inheritdoc />
    public partial class v140 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteTrip_Company_CompanyId",
                schema: "dbo",
                table: "RouteTrip");

            migrationBuilder.DropIndex(
                name: "IX_RouteTrip_CompanyId",
                schema: "dbo",
                table: "RouteTrip");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "RouteTrip");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "RouteTrip");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "Trip",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "Trip",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Trip_CompanyId",
                schema: "dbo",
                table: "Trip",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Company_CompanyId",
                schema: "dbo",
                table: "Trip",
                column: "CompanyId",
                principalSchema: "dbo",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Company_CompanyId",
                schema: "dbo",
                table: "Trip");

            migrationBuilder.DropIndex(
                name: "IX_Trip_CompanyId",
                schema: "dbo",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "Trip");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "dbo",
                table: "RouteTrip",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "dbo",
                table: "RouteTrip",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_RouteTrip_CompanyId",
                schema: "dbo",
                table: "RouteTrip",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteTrip_Company_CompanyId",
                schema: "dbo",
                table: "RouteTrip",
                column: "CompanyId",
                principalSchema: "dbo",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
