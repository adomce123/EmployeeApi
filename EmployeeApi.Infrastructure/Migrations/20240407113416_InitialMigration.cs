using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HomeAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmploymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: true),
                    CurrentSalary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_BossId",
                        column: x => x.BossId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Birthdate", "BossId", "CurrentSalary", "EmploymentDate", "FirstName", "HomeAddress", "LastName", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 200000, new DateTime(2010, 1, 1, 1, 1, 1, 0, DateTimeKind.Utc), "John", "123 Main St", "Doe", "Ceo" },
                    { 2, new DateTime(1985, 5, 15, 1, 0, 0, 0, DateTimeKind.Utc), 1, 150000, new DateTime(2012, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Jane", "456 Maple Ave", "Smith", "Manager" },
                    { 3, new DateTime(1990, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, 100000, new DateTime(2015, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bob", "789 Elm St", "Johnson", "Developer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BossId",
                table: "Employees",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Role",
                table: "Employees",
                column: "Role",
                unique: true,
                filter: "[Role] = 'Ceo'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
