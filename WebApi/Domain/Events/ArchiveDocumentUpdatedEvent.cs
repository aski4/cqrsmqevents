using MediatR;
using System;

namespace Domain.Events
{
    public class ArchiveDocumentUpdatedEvent : INotification
    {
        public Guid Id { get; }
        public string Text { get; }
        public DateTime DateUpdated { get; }

        public ArchiveDocumentUpdatedEvent(Guid id, string text, DateTime dateUpdated)
        {
            Id = id;
            Text = text;
            DateUpdated = dateUpdated;
        }

    }
}
