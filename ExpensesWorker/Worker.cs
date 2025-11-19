namespace ExpensesWorker
{
    public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var expensesService = scope.ServiceProvider.GetRequiredService<IExpensesService>();

                await expensesService.HandleExpensesItems();
            }
            

            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
