using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Abstraction;
using Infrastructure.Repository;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using System.Collections.Generic;

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

                  var exchangeCosumerOptions = new RabbitMqExchangeOptions
                  {
                      Queues = new List<RabbitMqQueueOptions>
                      {
                          new RabbitMqQueueOptions
                          {
                              Name = "web",
                              RoutingKeys = new HashSet<string> { "routing.key" }
                          }
                      }
                  };

                  services.AddRabbitMqClient(rabbitMqSection)
                      .AddConsumptionExchange("exchangeco.name", exchangeSection)
                      .AddProductionExchange("exchangepro.name", exchangeCosumerOptions)
                      .AddMessageHandlerTransient<DocProcessMessageHandler>("routing.key");

                  services.AddDbContext<ApplicationDbContext>(opt =>
                        opt.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                  services.AddScoped<ITempRepository, TempRepository>();
              });

            await builder.RunConsoleAsync();
        }
    }
}
