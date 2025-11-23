namespace ExpensesWorker.Domain
{
    internal class ExpenseValue
    {
        public Guid Id { get; set; }
        public int ExpenseItemId { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
