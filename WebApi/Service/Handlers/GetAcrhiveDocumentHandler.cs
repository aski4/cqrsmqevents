using Domain.Dtos;
using Domain.Queries;
using Infrastructure.Abstraction;
using Marten;
using MediatR;
using Service.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Handlers
{
    public class GetAcrhiveDocumentHandler : 
        IRequestHandler<GetArchiveDocumentQuery, ArchiveDocumentDto>,
        IRequestHandler<GetFinArchiveDocumentQuery, ArchiveDocumentDto>
    {
        private readonly IArchiveDocumentRepository _repository;
        private readonly IDocumentSession _session;

        public GetAcrhiveDocumentHandler(IArchiveDocumentRepository repository,
            IDocumentSession session)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async Task<ArchiveDocumentDto> Handle(GetArchiveDocumentQuery request, CancellationToken cancellationToken)
        {
            return (await _repository.GetAsync(x => x.Id == request.Id))?
                        .Map();
        }

        public async Task<ArchiveDocumentDto> Handle(GetFinArchiveDocumentQuery request, CancellationToken cancellationToken)
        {
            return await _session.LoadAsync<ArchiveDocumentDto>(request.Id);
        }
    }
}
