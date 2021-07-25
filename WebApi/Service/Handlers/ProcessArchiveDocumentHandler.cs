using Domain.Commands;
using Domain.Dtos;
using Domain.Events;
using Domain.Models;
using Infrastructure;
using Infrastructure.Abstraction;
using MediatR;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using Service.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Handlers
{
    public class ProcessArchiveDocumentHandler : 
        IRequestHandler<ProcessArchiveDocumentCommand, VoidClassDto>
    {
        private readonly IMediator _mediator;
        private readonly ITempRepository _tempRepository;
        private readonly IQueueService _queueService;

        public ProcessArchiveDocumentHandler(IMediator mediator, 
            IArchiveDocumentRepository repository,
            ITempRepository tempRepository,
            IQueueService queueService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _tempRepository = tempRepository ?? throw new ArgumentNullException(nameof(tempRepository));
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        }

        public async Task<VoidClassDto> Handle(ProcessArchiveDocumentCommand request, CancellationToken cancellationToken)
        {
            var tmpFile = new TempFile(request.FileBytes);
            _tempRepository.Add(tmpFile);
            await _tempRepository.SaveChangesAsync();

            var msg = new MessageMQ
            {
                Id = tmpFile.Id,
                EventName = nameof(ProcessArchiveDocumentCommand)
            };

            await _queueService.SendAsync(msg,
                                    "myq",
                                    "routing.key");

            return new VoidClassDto();
        }
    }
}
