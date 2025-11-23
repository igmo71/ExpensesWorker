using ExpensesWorker.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace ExpensesWorker.Application
{
    public interface ISellingExpenses79Service
    {
        Task HandleCsv();
    }

    internal class SellingExpenses79Service(IConfiguration configuration) : ISellingExpenses79Service
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task HandleCsv()
        {
            var filePath = _configuration.GetSection(nameof(SellingExpenses79)).Get<string>()
                ?? throw new InvalidOperationException("FilePath Not Found");

            var records = await CsvReader_SellingExpenses79.ParseCsv(filePath);

            var table = ToDataTable(records);

            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionString Not Found");

            using var connection = new SqlConnection(connectionString);

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.Expense_Затраты_по_реализациям_79"
            };

            await bulk.WriteToServerAsync(table);
        }

        private DataTable ToDataTable(List<SellingExpenses79> data)
        {
            var table = new DataTable();

            foreach (var prop in typeof(SellingExpenses79).GetProperties())
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var row in data)
            {
                var values = typeof(SellingExpenses79).GetProperties()
                    .Select(p => p.GetValue(row) ?? DBNull.Value)
                    .ToArray();
                table.Rows.Add(values);
            }

            return table;
        }
    }
}
