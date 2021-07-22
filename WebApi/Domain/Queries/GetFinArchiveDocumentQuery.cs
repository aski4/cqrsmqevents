using Domain.Dtos;
using System;

namespace Domain.Queries
{
    public class GetFinArchiveDocumentQuery : QueryBase<ArchiveDocumentDto>
    {
        public GetFinArchiveDocumentQuery() { }
                  
        public GetFinArchiveDocumentQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
