using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessConsole
{
    public class ConsumingService : IHostedService
    {
        readonly IQueueService _queueService;
        readonly ILogger<ConsumingService> _logger;

        public ConsumingService(
            IQueueService queueService,
            ILogger<ConsumingService> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting consuming.");
            _queueService.StartConsuming();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping consuming.");
            _queueService.StopConsuming();
            return Task.CompletedTask;
        }
    }
}
