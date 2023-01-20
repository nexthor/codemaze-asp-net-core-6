using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7851446f-301d-4dd7-bede-f0ae290df36c", "3454e862-48d0-4629-9dbf-8d6b0fec14a5", "Manager", "MANAGER" },
                    { "87009462-ff4f-4c43-ac04-07bec3311206", "e550bcb2-f2a4-4178-8d53-56a791292ffc", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7851446f-301d-4dd7-bede-f0ae290df36c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87009462-ff4f-4c43-ac04-07bec3311206");
        }
    }
}
