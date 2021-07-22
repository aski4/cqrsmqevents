using Infrastructure.Abstraction;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessConsole
{
    public class DocProcessMessageHandler : IMessageHandler
    {
        private readonly ITempRepository _tempRepository;
        private readonly ILogger<DocProcessMessageHandler> _logger;

        public DocProcessMessageHandler(ITempRepository tempRepository,
            ILogger<DocProcessMessageHandler> logger)
        {
            _tempRepository = tempRepository ?? throw new ArgumentNullException(nameof(tempRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Handling message {message} by routing key {matchingRoute}");
        }
    }
}
