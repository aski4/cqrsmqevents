using Domain.Events;
using Marten;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Handlers.Events
{
    public class EventsHandler : 
        INotificationHandler<ArchiveDocumentProcessedEvent>,
        INotificationHandler<ArchiveDocumentUpdatedEvent>
    {
        private readonly IDocumentSession _session;

        public EventsHandler(IDocumentSession session)
        {
            _session = session ?? throw new ArgumentException(nameof(session));
        }

        public async Task Handle(ArchiveDocumentProcessedEvent notification, CancellationToken cancellationToken)
        {
            _session.Events.Append(notification.Id, notification);
            await _session.SaveChangesAsync();
        }

        public async Task Handle(ArchiveDocumentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _session.Events.Append(notification.Id, notification);
            await _session.SaveChangesAsync();
        }
    }
}
