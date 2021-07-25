using Domain.Commands;
using Domain.Events;
using Infrastructure;
using Infrastructure.Abstraction;
using Microsoft.Extensions.Logging;
using OCR;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace ProcessConsole
{
    public class DocProcessMessageHandler : IMessageHandler
    {
        private readonly ITempRepository _tempRepository;
        private readonly IArchiveDocumentRepository _archiveDocument;
        private readonly ILogger<DocProcessMessageHandler> _logger;

        public DocProcessMessageHandler(ITempRepository tempRepository,
            IArchiveDocumentRepository archiveDocument,
            ILogger<DocProcessMessageHandler> logger)
        {
            _tempRepository = tempRepository ?? throw new ArgumentNullException(nameof(tempRepository));
            _archiveDocument = archiveDocument ?? throw new ArgumentNullException(nameof(archiveDocument));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        {
            var body = eventArgs.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            _logger.LogWarning($"Handling message {jsonString} by routing key {matchingRoute}");
            var mq = JsonSerializer.Deserialize<MessageMQ>(jsonString);

            if (mq.EventName == nameof(ProcessArchiveDocumentCommand))
            {
                var tmpFile = _tempRepository.GetAsync(x => x.Id == mq.Id).GetAwaiter().GetResult();

                if (tmpFile == null)
                {
                    _logger.LogError($"tmpFile not found id:{mq.Id}");
                    return;
                }

                var ocr = new CoreOCR(LanguageEnum.Russian);
                var result = ocr.GetTextFromBytes(tmpFile.FileBytes);

                _archiveDocument.Add(new Domain.Models.ArchiveDocument(tmpFile.Id, "none", result.Text));
                _tempRepository.Remove(tmpFile);

                _tempRepository.SaveChangesAsync();

                _logger.LogWarning($"{tmpFile.Id} file have been processed");
            }

        }
    }
}
