using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class ArchiveDocumentUpdateCommand : CommandBase<ArchiveDocumentDto>
    {
        public ArchiveDocumentUpdateCommand() { }

        public ArchiveDocumentUpdateCommand(Guid id, string text)
        {
            Id = id;
            Text = text;
        }

        public Guid Id { get; }
        public string Text { get; }
    }
}
