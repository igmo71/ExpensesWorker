using ClosedXML.Excel;
using ExpensesWorker.Domain;

namespace ExpensesWorker.Application;

internal class ExcelReader_РL_2025
{
    public static (List<ExpenseItem> Items, List<ExpenseValue> Values)
    ParseExcel(string filePath, string[] allowedRoots)
    {
        var workbook = new XLWorkbook(filePath);
        var ws = workbook.Worksheets.First();

        // 1 --- Парсим месячные даты
        var monthHeaders = ParseMonthHeaders(ws);

        // 2 --- Парсим иерархию (первая колонка)
        var items = ParseItems(ws, allowedRoots);

        // 3 --- Парсим значения (остальные колонки)
        var values = ParseItemValues(ws, items, monthHeaders);

        return (items, values);
    }

    private static List<DateTime> ParseMonthHeaders(IXLWorksheet ws)
    {
        return ws.Row(3)
            .CellsUsed()
            .Skip(2)
            .Select(c =>
            {
                // 1: Прямая дата (или формула, вернувшая дату)
                if (c.TryGetValue(out DateTime dt))
                    return dt.Date;

                // 2: Формула → число → OADate
                if (c.HasFormula && c.TryGetValue(out double dbl))
                    return DateTime.FromOADate(dbl).Date;

                // 3: Число → год (2025)
                if (c.TryGetValue(out int year))
                    return new DateTime(year, 1, 1);

                throw new Exception($"Невозможно преобразовать дату в ячейке: {c.Address}");
            })
            .ToList();
    }

    private static List<ExpenseItem> ParseItems(IXLWorksheet ws, string[] allowedRoots)
    {
        var items = new List<ExpenseItem>();
        var stack = new Dictionary<int, ExpenseItem>();
        bool inAllowedBlock = false;

        int nextId = 1;

        foreach (var row in ws.RowsUsed())
        {
            var cell = row.Cell(1);
            var text = cell.GetString().Trim();
            if (string.IsNullOrWhiteSpace(text))
                continue;

            int level = cell.Style.Alignment.Indent;

            // Определяем начало нужного блока
            if (level == 0)
            {
                inAllowedBlock = allowedRoots.Contains(text);
                if (!inAllowedBlock)
                {
                    stack.Clear();
                    continue;
                }
            }

            if (!inAllowedBlock)
                continue;

            var item = new ExpenseItem
            {
                Id = nextId++,
                Name = text,
                Level = level,
                ParentId = (level == 0)
                    ? null
                    : stack.ContainsKey(level - 1)
                        ? stack[level - 1].Id
                        : null
            };

            items.Add(item);
            stack[level] = item;

            foreach (var k in stack.Keys.Where(k => k > level).ToList())
                stack.Remove(k);
        }

        return items;
    }

    private static List<ExpenseValue> ParseItemValues(
     IXLWorksheet ws,
     List<ExpenseItem> items,
     List<DateTime> months)
    {
        var values = new List<ExpenseValue>();

        foreach (var row in ws.RowsUsed())
        {
            var name = row.Cell(1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var item = items.FirstOrDefault(i => i.Name == name);
            if (item == null)
                continue;

            var dataCells = row.CellsUsed().Skip(1).ToList();

            for (int i = 0; i < months.Count && i < dataCells.Count; i++)
            {
                var c = dataCells[i];

                double val = 0;

                // 1) нормальное число
                if (c.TryGetValue(out double dbl))
                    val = dbl;

                // 2) формула → computed value
                else if (c.HasFormula && c.CachedValue.Type == XLDataType.Number)
                    val = c.CachedValue.GetNumber();

                // 3) целое (редко)
                else if (c.TryGetValue(out int intVal))
                    val = intVal;

                // 4) пустые → 0 (оставляем)

                values.Add(new ExpenseValue
                {
                    ExpenseItemId = item.Id,
                    DateTime = months[i],
                    Value = val
                });
            }
        }

        return values;
    }
}
