using ExpensesWorker.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensesWorker.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<ExpenseValue> ExpenseValues { get; set; }

        public DbSet<SellingExpenses> SellingExpenses79 { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseItem>().HasKey(e => e.Id);
            modelBuilder.Entity<ExpenseItem>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<ExpenseValue>().HasKey(e => e.Id);

            modelBuilder.Entity<SellingExpenses>().ToTable("Expense_Затраты_по_реализациям");
            modelBuilder.Entity<ExpenseItem>().HasKey(e => e.Id);
            modelBuilder.Entity<SellingExpenses>().Property(e => e.Id).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().HasKey(e => new
            //{
            //    e._year,
            //    e._month,
            //    e.Подразделение,
            //    e.НаборСтатей,
            //    e.Статья,
            //    e.СпособДоставки,
            //    e.КаналРеализации,
            //    e.Склад,
            //    e.Номенклатура_Key
            //});
            //modelBuilder.Entity<SellingExpenses>().Property(e => e._year).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e._month).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.Подразделение).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.НаборСтатей).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.Статья).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.СпособДоставки).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.КаналРеализации).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.Склад).ValueGeneratedNever();
            //modelBuilder.Entity<SellingExpenses>().Property(e => e.Номенклатура_Key).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
