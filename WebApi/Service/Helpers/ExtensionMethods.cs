using Domain.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public static class ExtensionMethods
    {
        public static ArchiveDocumentDto Map(this ArchiveDocument model)
        {
            return new ArchiveDocumentDto
            {
                Id = model.Id,
                FileName = model.FileName,
                Text = model.Text,
                DateUpdated = model.DateUpdated
            };
        }
    }
}
