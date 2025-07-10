using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mission.Entities.Migrations
{
    /// <inheritdoc />
    public partial class updatedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "CreatedDate", "email_address", "first_name", "IsDeleted", "last_name", "ModifiedDate", "password", "phone_number", "user_image", "user_type" },
                values: new object[] { 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@tatvasoft.com", "Tatva", false, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tatva@123", "9876543210", "", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "CreatedDate", "email_address", "first_name", "IsDeleted", "last_name", "ModifiedDate", "password", "phone_number", "user_image", "user_type" },
                values: new object[] { 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@tatvasoft.com", "Tatva", false, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tatva@123", "9876543210", "", "admin" });
        }
    }
}
