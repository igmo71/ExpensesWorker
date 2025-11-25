using ExpensesWorker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesWorker.Application
{
    public interface IExpensesService
    {
        Task HandleExcel();
    }

    internal class ExpensesService(AppDbContext dbContext, IConfiguration configuration) : IExpensesService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task HandleExcel()
        {
            await _dbContext.ExpenseItems.ExecuteDeleteAsync();
            await _dbContext.ExpenseValues.ExecuteDeleteAsync();

            var allowedRoots = _configuration.GetSection("AllowedRoots").Get<string[]>()
                ?? throw new InvalidOperationException("AllowedRoots Not Found");

            var filePath = _configuration.GetSection("ExpensesFilePath").Get<string>()
                ?? throw new InvalidOperationException("FilePath Not Found");

            var result = ExpensesExcelReader.ParseExcel(filePath, allowedRoots);

            await _dbContext.ExpenseItems.AddRangeAsync(result.Items);

            await _dbContext.ExpenseValues.AddRangeAsync(result.Values);

            await _dbContext.SaveChangesAsync();
        }
    }
}
