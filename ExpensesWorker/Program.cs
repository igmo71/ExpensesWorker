using ExpensesWorker.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpensesWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new ApplicationException("DefaultConnection not found");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IExpensesService, ExpensesService>();

            var host = builder.Build();
            host.Run();
        }
    }
}
