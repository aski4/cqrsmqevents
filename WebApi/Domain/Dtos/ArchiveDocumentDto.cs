using Domain.Events;
using System;

namespace Domain.Dtos
{
    public class ArchiveDocumentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Text { get; set; }
        public DateTime DateUpdated { get; set; }

        public void Apply(ArchiveDocumentUpdatedEvent @event)
        {
            Text = @event.Text;
            DateUpdated = @event.DateUpdated;
        }
    }
}
