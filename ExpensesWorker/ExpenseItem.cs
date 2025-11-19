using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesWorker
{
    internal class ExpenseItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int? ParentId { get; set; }
    }
}
