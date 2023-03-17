using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationCompany.Migrations
{
    /// <inheritdoc />
    public partial class v141 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_RouteTrip_RouteTripId",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_RouteTripId",
                schema: "dbo",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "RouteTripId",
                schema: "dbo",
                table: "Vehicle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RouteTripId",
                schema: "dbo",
                table: "Vehicle",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_RouteTripId",
                schema: "dbo",
                table: "Vehicle",
                column: "RouteTripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_RouteTrip_RouteTripId",
                schema: "dbo",
                table: "Vehicle",
                column: "RouteTripId",
                principalSchema: "dbo",
                principalTable: "RouteTrip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
