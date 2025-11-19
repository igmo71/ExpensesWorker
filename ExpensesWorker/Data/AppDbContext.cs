using Microsoft.EntityFrameworkCore;

namespace ExpensesWorker.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<ExpenseItem> ExpenseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseItem>().HasKey(e => e.Id);
            modelBuilder.Entity<ExpenseItem>().Property(e => e.Id).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
