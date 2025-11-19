using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesWorker
{
    internal class ExcelReader
    {
        public static List<ExpenseItem> ParseFirstColumn(string filePath, string[] allowedRoots)
        {
            var workbook = new XLWorkbook(filePath);
            var ws = workbook.Worksheets.First();

            var nodes = new List<ExpenseItem>();
            var stack = new Dictionary<int, ExpenseItem>(); // уровень → последний узел

            bool inAllowedBlock = false;

            foreach (var row in ws.RowsUsed())
            {
                var cell = row.Cell(1);
                var text = cell.GetString().Trim();

                if (string.IsNullOrWhiteSpace(text))
                    continue;

                // Получаем уровень (0, 1, 2, ...)
                int level = cell.Style.Alignment.Indent;

                // ===== ЛОГИКА: определяем, находимся ли мы внутри нужного раздела =====
                if (level == 0)
                {
                    // Является ли раздел корнем-верхушкой?
                    inAllowedBlock = allowedRoots.Contains(text);

                    // Если корень не из списка → пропускаем всю ветку
                    if (!inAllowedBlock)
                    {
                        stack.Clear();
                        continue;
                    }
                }

                // если мы не в нужном блоке → пропустить
                if (!inAllowedBlock)
                    continue;

                // ===== Создаём узел =====
                var node = new ExpenseItem
                {
                    Id = nodes.Count + 1,
                    Name = text,
                    Level = level,
                    ParentId = (level == 0)
                        ? null
                        : stack.ContainsKey(level - 1)
                            ? stack[level - 1].Id
                            : null
                };

                nodes.Add(node);

                // обновляем текущий уровень
                stack[level] = node;

                // убираем уровни глубже
                var toRemove = stack.Keys.Where(k => k > level).ToList();
                foreach (var r in toRemove)
                    stack.Remove(r);
            }

            return nodes;
        }
    }
}
