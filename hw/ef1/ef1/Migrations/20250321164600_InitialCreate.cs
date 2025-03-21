using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ef1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceHistories_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "Model", "Price", "Year" },
                values: new object[,]
                {
                    { 1, "Toyota", "Camry", 2500000m, 2022 },
                    { 2, "BMW", "X5", 4500000m, 2021 },
                    { 3, "Mercedes", "C-Class", 3500000m, 2023 },
                    { 4, "Audi", "A6", 3700000m, 2022 },
                    { 5, "Ford", "Focus", 1800000m, 2020 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "ivan@test.com", "Иван Иванов", "111-111" },
                    { 2, "olga@test.com", "Ольга Петрова", "222-222" },
                    { 3, "alexey@test.com", "Алексей Смирнов", "333-333" },
                    { 4, "elena@test.com", "Елена Козлова", "444-444" },
                    { 5, "dmitry@test.com", "Дмитрий Васильев", "555-555" },
                    { 6, "anna@test.com", "Анна Михайлова", "666-666" },
                    { 7, "sergey@test.com", "Сергей Кузнецов", "777-777" },
                    { 8, "marina@test.com", "Марина Попова", "888-888" },
                    { 9, "artem@test.com", "Артем Соколов", "999-999" },
                    { 10, "yulia@test.com", "Юлия Николаева", "101-101" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Name", "Position" },
                values: new object[,]
                {
                    { 1, "Олег Сидоров", "Менеджер" },
                    { 2, "Мария Алексеева", "Менеджер" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "CarId", "CustomerId", "EmployeeId", "SaleDate" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2, 2, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 3, 1, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, 4, 2, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, 5, 1, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CarId",
                table: "Sales",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerId",
                table: "Sales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_EmployeeId",
                table: "Sales",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_CarId",
                table: "ServiceHistories",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "ServiceHistories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
