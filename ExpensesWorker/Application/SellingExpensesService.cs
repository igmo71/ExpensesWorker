using CsvHelper;
using CsvHelper.Configuration;
using ExpensesWorker.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace ExpensesWorker.Application
{
    public interface ISellingExpensesService
    {
        Task HandleCsv();
    }

    internal class SellingExpensesService(IConfiguration configuration, ILogger<SellingExpensesService> logger) : ISellingExpensesService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<SellingExpensesService> _logger = logger;

        public async Task HandleCsv()
        {
            var startingTimestamp = Stopwatch.GetTimestamp();

            var filePath = _configuration.GetSection("SellingExpensesFilePath").Get<string>()
                ?? throw new InvalidOperationException("FilePath Not Found");

            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionString Not Found");

            int batchSize = _configuration.GetSection("BatchSize").Get<int>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null,
                IgnoreBlankLines = true
            };

            using var reader = new StreamReader(filePath);

            using var csv = new CsvReader(reader, config);

            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.Expense_Затраты_по_реализациям",
                BatchSize = batchSize,
                BulkCopyTimeout = 0            
            };

            var table = CreateTableSchema();   
            int counter = 0;
            int total = 0;

            await foreach (var raw in csv.GetRecordsAsync<SellingExpensesCsv>())
            {
                var row = table.NewRow();

                row["Id"] = Guid.CreateVersion7();
                row["_year"] = raw._year;
                row["_month"] = raw._month;
                row["Подразделение"] = raw.Подразделение;
                row["НаборСтатей"] = raw.НаборСтатей;
                row["Статья"] = raw.Статья;
                row["СпособДоставки"] = raw.СпособДоставки;
                row["КаналРеализации"] = raw.КаналРеализации;
                row["Склад"] = raw.Склад;
                row["year_month"] = raw.year_month;
                row["Номенклатура_Key"] =raw.Номенклатура_Key;
                row["Расчетное_Значение"] =raw.Расчетное_Значение;

                table.Rows.Add(row);
                counter++;

                if (counter == batchSize)
                {
                    await bulk.WriteToServerAsync(table);
                    table.Clear();
                    total += counter;
                    counter = 0;
                    _logger.LogDebug("Inserted: {Total} in {Elapsed}", total, Stopwatch.GetElapsedTime(startingTimestamp));
                }
            }

            if (counter > 0)
            {
                await bulk.WriteToServerAsync(table);
                total += counter;
            }

            _logger.LogDebug("Total Inserted: {Total} in {Elapsed}", total, Stopwatch.GetElapsedTime(startingTimestamp));
        }

        private static DataTable CreateTableSchema()
        {
            var dt = new DataTable();

            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("_year", typeof(int));
            dt.Columns.Add("_month", typeof(int));
            dt.Columns.Add("Подразделение", typeof(string));
            dt.Columns.Add("НаборСтатей", typeof(string));
            dt.Columns.Add("Статья", typeof(string));
            dt.Columns.Add("СпособДоставки", typeof(string));
            dt.Columns.Add("КаналРеализации", typeof(string));
            dt.Columns.Add("Склад", typeof(string));
            dt.Columns.Add("year_month", typeof(string));
            dt.Columns.Add("Номенклатура_Key", typeof(string));
            dt.Columns.Add("Расчетное_Значение", typeof(double));

            return dt;
        }

        private static double ParseDouble(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return 0;

            s = s.Replace(" ", "").Replace(",", ".");

            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            return 0;
        }
    }
}
