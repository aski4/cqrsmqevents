using Domain.Dtos;
using Domain.Events;
using Infrastructure;
using Infrastructure.Abstraction;
using Infrastructure.Repository;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using Service;
using Service.Handlers;
using System.Collections.Generic;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                 opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddScoped<IArchiveDocumentRepository, ArchiveDocumentRepository>();
            services.AddScoped<ITempRepository, TempRepository>();

            services.AddMediatR(typeof(ProcessArchiveDocumentHandler),
                                typeof(GetAcrhiveDocumentHandler));

            services.AddScoped(_ =>
            {
                var documentStore = DocumentStore.For(options =>
                {
                    var config = Configuration.GetSection("EventStore");
                    var connectionString = config.GetValue<string>("ConnectionString");
                    var schemaName = config.GetValue<string>("Schema");

                    options.Connection(connectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Events.DatabaseSchemaName = schemaName;
                    options.DatabaseSchemaName = schemaName;

                    options.Events.InlineProjections.AggregateStreamsWith<ArchiveDocumentDto>();

                    options.Events.AddEventType(typeof(ArchiveDocumentProcessedEvent));
                    options.Events.AddEventType(typeof(ArchiveDocumentUpdatedEvent));

                });

                return documentStore.OpenSession();
            });

            var exchangeProducerOptions = new RabbitMqExchangeOptions
            {
                Queues = new List<RabbitMqQueueOptions>
                      {
                          new RabbitMqQueueOptions
                          {
                              Name = "console",
                              RoutingKeys = new HashSet<string> { "routing.key" }
                          }
                      }
            };
            var rabbitMqSection = Configuration.GetSection("RabbitMq");
            var exchangeSection = Configuration.GetSection("RabbitMqExchange");

            services.AddRabbitMqClient(rabbitMqSection)
                      .AddConsumptionExchange("exchangeco.name", exchangeSection)
                      .AddProductionExchange("exchangepro.name", exchangeProducerOptions)
                      .AddMessageHandlerTransient<DocWebProcessMessageHandler>("routing.key");

            services.AddHostedService<ConsumingWebService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.Migrate();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
