using ExpensesWorker.Application;

namespace ExpensesWorker
{
    public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workerDelay = configuration.GetSection("WorkerDelay").Get<int>();

            while (!stoppingToken.IsCancellationRequested)
            {
                //await LoadExpenses();

                await LoadSellingExpenses79();

                await Task.Delay(1000 * workerDelay, stoppingToken);
            }
        }

        private async Task LoadExpenses()
        {
            using var scope = scopeFactory.CreateScope();

            var expensesService = scope.ServiceProvider.GetRequiredService<IExpensesService>();

            await expensesService.HandleExcel();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Handle Excel at: {time}", DateTimeOffset.Now);
            }
        }

        private async Task LoadSellingExpenses79()
        {
            using var scope = scopeFactory.CreateScope();

            var expensesService = scope.ServiceProvider.GetRequiredService<ISellingExpenses79Service>();

            await expensesService.HandleCsv();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Handle Excel at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
