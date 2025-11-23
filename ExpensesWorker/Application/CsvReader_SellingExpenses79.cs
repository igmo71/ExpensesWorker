using CsvHelper;
using CsvHelper.Configuration;
using ExpensesWorker.Domain;
using System.Globalization;

namespace ExpensesWorker.Application;


internal class CsvReader_SellingExpenses79
{
    public static async Task<List<SellingExpenses79>> ParseCsv(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null,
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        var list = new List<SellingExpenses79>();

        await foreach (var record in csv.GetRecordsAsync<RowModel_SellingExpenses79>())
        {
            var row = new SellingExpenses79
            {
                _year = record._year,
                _month = record._month,
                Подразделение = record.Подразделение,
                НаборСтатей = record.НаборСтатей,
                Статья = record.Статья,
                Сумма = ParseDouble(record.Сумма),
                CALCBASE = record.CALCBASE,
                РублиЗаКг = record.РублиЗаКг,
                СпособДоставки = record.СпособДоставки,
                КаналРеализации = record.КаналРеализации,
                Склад = record.Склад,
                year_month = record.year_month,
                Номенклатура_Key = record.Номенклатура_Key,
                Количество = record.Количество,
                Вес = record.Вес,
                СуммаВыручки = record.СуммаВыручки,
                Расчетное_Значение = record.Расчетное_Значение
            };

            list.Add(row);
        }

        return list;
    }

    private static double ParseDouble(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return 0;

        s = s.Replace(" ", "").Replace(",", ".");

        if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        return 0;
    }
}


internal class RowModel_SellingExpenses79
{
    public int _year { get; set; }
    public int _month { get; set; }
    public string? Подразделение { get; set; }
    public string? НаборСтатей { get; set; }
    public string? Статья { get; set; }
    public string? Сумма { get; set; }
    public double CALCBASE { get; set; }
    public double РублиЗаКг { get; set; }
    public string? СпособДоставки { get; set; }
    public string? КаналРеализации { get; set; }
    public string? Склад { get; set; }

    public string? year_month { get; set; }
    public Guid Номенклатура_Key { get; set; }
    public double Количество { get; set; }
    public double Вес { get; set; }
    public double СуммаВыручки { get; set; }
    public double Расчетное_Значение { get; set; }
}