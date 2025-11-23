using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesWorker.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSellingExpenses79 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expense_Затраты_по_реализациям_79",
                columns: table => new
                {
                    _year = table.Column<int>(type: "int", nullable: false),
                    _month = table.Column<int>(type: "int", nullable: false),
                    Подразделение = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    НаборСтатей = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Статья = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    СпособДоставки = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    КаналРеализации = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Склад = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Номенклатура_Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Сумма = table.Column<double>(type: "float", nullable: false),
                    CALCBASE = table.Column<double>(type: "float", nullable: false),
                    РублиЗаКг = table.Column<double>(type: "float", nullable: false),
                    year_month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Количество = table.Column<double>(type: "float", nullable: false),
                    Вес = table.Column<double>(type: "float", nullable: false),
                    СуммаВыручки = table.Column<double>(type: "float", nullable: false),
                    Расчетное_Значение = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense_Затраты_по_реализациям_79", x => new { x._year, x._month, x.Подразделение, x.НаборСтатей, x.Статья, x.СпособДоставки, x.КаналРеализации, x.Склад, x.Номенклатура_Key });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense_Затраты_по_реализациям_79");
        }
    }
}
