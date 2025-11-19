using ExpensesWorker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesWorker
{
    public interface IExpensesService
    {
        Task HandleExpensesItems();
    }

    internal class ExpensesService(AppDbContext dbContext, IConfiguration configuration) : IExpensesService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task HandleExpensesItems()
        {
            await _dbContext.ExpenseItems.ExecuteDeleteAsync();

            var allowedRoots = _configuration.GetSection("AllowedRoots").Get<string[]>()
                ?? throw new InvalidOperationException("AllowedRoots Not Found");

            var nodes = ExcelReader.ParseFirstColumn("Data/РL_2025.xlsx", allowedRoots);

            await _dbContext.AddRangeAsync(nodes);

            await _dbContext.SaveChangesAsync();
        }
    }
}
