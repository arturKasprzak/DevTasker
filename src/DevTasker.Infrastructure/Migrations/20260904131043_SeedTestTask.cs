using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedUserId", "Description", "Status", "Title" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), null, true, "Add TextBox" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
