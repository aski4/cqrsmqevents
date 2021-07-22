using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ArchiveDocument : BaseModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public ArchiveDocument(Guid id, string fileName, string text)
        {
            Id = id;
            FileName = fileName;
            Text = text;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
        }

    }
}
