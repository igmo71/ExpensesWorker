namespace ExpensesWorker.Domain
{
    internal class SellingExpenses
    {
        public Guid Id { get; set; }
        public int _year { get; set; }
        public int _month { get; set; }
        public string? Подразделение { get; set; }
        public string? НаборСтатей { get; set; }
        public string? Статья { get; set; }        
        public string? СпособДоставки { get; set; }
        public string? КаналРеализации { get; set; }
        public string? Склад { get; set; }
        public string? year_month { get; set; }
        public string? Номенклатура_Key { get; set; }
        public double Расчетное_Значение { get; set; }
    }

    internal class SellingExpensesCsv
    {
        public int _year { get; set; }
        public int _month { get; set; }
        public string? Подразделение { get; set; }
        public string? НаборСтатей { get; set; }
        public string? Статья { get; set; }
        public string? Сумма { get; set; }
        public string? CALCBASE { get; set; }
        public string? РублиЗаКг { get; set; }
        public string? СпособДоставки { get; set; }
        public string? КаналРеализации { get; set; }
        public string? Склад { get; set; }
        public string? year_month { get; set; }
        public string? Номенклатура_Key { get; set; }
        public string? Количество { get; set; }
        public string? Вес { get; set; }
        public string? СуммаВыручки { get; set; }
        public double Расчетное_Значение { get; set; }
    }
}
