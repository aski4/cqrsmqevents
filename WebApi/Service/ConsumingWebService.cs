using MediatR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class ConsumingWebService : IHostedService
    {
        private readonly IQueueService _queueService;

        public ConsumingWebService(IQueueService queueService, IMediator mediator)
        {
            _queueService = queueService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _queueService.StartConsuming();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _queueService.StopConsuming();
            return Task.CompletedTask;
        }
    }
}
