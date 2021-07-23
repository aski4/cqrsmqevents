using Domain.Commands;
using Domain.Events;
using Infrastructure;
using MediatR;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Service.Handlers
{
    public class DocWebProcessMessageHandler : IMessageHandler
    {
        private readonly IMediator _mediator;

        public DocWebProcessMessageHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        {
            var body = eventArgs.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            var mq = JsonSerializer.Deserialize<MessageMQ>(jsonString);

            if (mq.EventName == nameof(ArchiveDocumentUpdateCommand))
            {
                _mediator.Send(new ArchiveDocumentUpdateCommand(mq.Id, mq.EventName));
            }
        }
    }
}
