using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Queries
{
    public class GetArchiveDocumentQuery : QueryBase<ArchiveDocumentDto>
    {
        public GetArchiveDocumentQuery() { }

        public GetArchiveDocumentQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
