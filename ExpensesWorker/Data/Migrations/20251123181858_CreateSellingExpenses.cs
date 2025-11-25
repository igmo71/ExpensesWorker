using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesWorker.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSellingExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expense_Затраты_по_реализациям",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _year = table.Column<int>(type: "int", nullable: false),
                    _month = table.Column<int>(type: "int", nullable: false),
                    Подразделение = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    НаборСтатей = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Статья = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    СпособДоставки = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    КаналРеализации = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Склад = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    year_month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Номенклатура_Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Расчетное_Значение = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense_Затраты_по_реализациям", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense_Затраты_по_реализациям");
        }
    }
}
