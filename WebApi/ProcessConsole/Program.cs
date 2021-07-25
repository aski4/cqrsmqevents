using Infrastructure;
using Infrastructure.Abstraction;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using System.Threading.Tasks;

namespace ProcessConsole
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = new HostBuilder()
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
              })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
              })
              .ConfigureServices((hostContext, services) =>
              {
                  var rabbitMqSection = hostContext.Configuration.GetSection("RabbitMq");
                  var exchangeSection = hostContext.Configuration.GetSection("RabbitMqExchange");


                  services.AddRabbitMqClient(rabbitMqSection)
                      .AddConsumptionExchange("myq", exchangeSection)
                      .AddMessageHandlerSingleton<DocProcessMessageHandler>("routing.key");


                  services.AddDbContext<ApplicationDbContext>(opt =>
                        opt.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                  services.AddScoped<ITempRepository, TempRepository>();
                  services.AddScoped<IArchiveDocumentRepository, ArchiveDocumentRepository>();
                  services.AddSingleton<IHostedService, ConsumingService>();

              });


            await builder.RunConsoleAsync();
        }

    }
}
