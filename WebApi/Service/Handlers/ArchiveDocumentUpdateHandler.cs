using Domain.Commands;
using Domain.Dtos;
using Domain.Events;
using Infrastructure.Abstraction;
using MediatR;
using Service.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Handlers
{
    public class ArchiveDocumentUpdateHandler : IRequestHandler<ArchiveDocumentUpdateCommand, ArchiveDocumentDto>
    {
        private readonly IArchiveDocumentRepository _repository;
        private readonly IMediator _mediator;

        public ArchiveDocumentUpdateHandler(IArchiveDocumentRepository repository,
            IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<ArchiveDocumentDto> Handle(ArchiveDocumentUpdateCommand request, CancellationToken cancellationToken)
        {
            var doc = await _repository.GetAsync(x => x.Id == request.Id);
            if (doc is null)
                throw new Exception($"Requested document id:{request.Id} not found");

            doc.Text = request.Text;
            doc.DateUpdated = DateTime.UtcNow;

            await _mediator.Publish(new ArchiveDocumentUpdatedEvent(doc.Id, doc.Text, doc.DateUpdated));

            return doc.Map();
        }
    }
}
