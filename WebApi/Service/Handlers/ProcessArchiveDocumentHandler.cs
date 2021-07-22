using Domain.Commands;
using Domain.Dtos;
using Domain.Events;
using Domain.Models;
using Infrastructure.Abstraction;
using MediatR;
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

        public ProcessArchiveDocumentHandler(IMediator mediator, 
            IArchiveDocumentRepository repository,
            ITempRepository tempRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _tempRepository = tempRepository ?? throw new ArgumentNullException(nameof(tempRepository));
        }

        public async Task<VoidClassDto> Handle(ProcessArchiveDocumentCommand request, CancellationToken cancellationToken)
        {
            //var ocr = new CoreOCR(LanguageEnum.Russian);
            //var result = await ocr.GetTextFromBytesAsync(request.FileBytes);

            //if (result is null)
            //    throw new Exception("Unable to read the file");
            //
            _tempRepository.Add(new TempFile(request.FileBytes));
            await _tempRepository.SaveChangesAsync();

            //var archDocument = new ArchiveDocument(request.FileName, "");// result.Text);

            //_repository.Add(archDocument);
            //await _repository.SaveChangesAsync();
            //
            //await _mediator.Publish(new ArchiveDocumentProcessedEvent(archDocument.Id));

            return new VoidClassDto();
        }
    }
}
