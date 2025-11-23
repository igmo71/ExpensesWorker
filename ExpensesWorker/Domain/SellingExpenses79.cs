namespace ExpensesWorker.Domain
{
    internal class SellingExpenses79
    {
        public int _year { get; set; }
        public int _month { get; set; }
        public string? Подразделение { get; set; }
        public string? НаборСтатей { get; set; }
        public string? Статья { get; set; }
        public double Сумма { get; set; }
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
}
