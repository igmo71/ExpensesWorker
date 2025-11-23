using System.ComponentModel.DataAnnotations;

namespace ExpensesWorker.Domain
{
    internal class ExpenseItem
    {
        public int Id { get; set; }

        [MaxLength(400)]
        public string? Name { get; set; }

        public int Level { get; set; }

        public int? ParentId { get; set; }
    }
}
