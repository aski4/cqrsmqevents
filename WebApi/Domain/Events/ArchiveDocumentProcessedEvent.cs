using MediatR;
using System;

namespace Domain.Events
{
    public class ArchiveDocumentProcessedEvent : INotification
    {
        public Guid Id { get; }

        public ArchiveDocumentProcessedEvent(Guid id)
        {
            Id = id;
        }
    }
}
