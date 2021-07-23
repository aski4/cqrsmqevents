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
    public class DocProcessMessageHandler : INonCyclicMessageHandler
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

        //public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        //{
        //    var body = eventArgs.Body.ToArray();
        //    var jsonString = Encoding.UTF8.GetString(body);

        //    _logger.LogWarning($"Handling message {jsonString} by routing key {matchingRoute}");
        //    var mq = JsonSerializer.Deserialize<MessageMQ>(jsonString);

        //    if(mq.EventName == nameof(ProcessArchiveDocumentCommand))
        //    {
        //        var tmpFile = _tempRepository.GetAsync(x => x.Id == mq.Id).GetAwaiter().GetResult();

        //        if (tmpFile == null)
        //        {
        //            _logger.LogError($"tmpFile not found id:{mq.Id}");
        //            return;
        //        }

        //        var ocr = new CoreOCR(LanguageEnum.Russian);
        //        var result = ocr.GetTextFromBytes(tmpFile.FileBytes);

        //        _archiveDocument.Add(new Domain.Models.ArchiveDocument(tmpFile.Id, "none", result.Text));
        //        _archiveDocument.SaveChangesAsync();

        //        _tempRepository.Remove(tmpFile);
        //        _tempRepository.SaveChangesAsync();

                
        //        _sendService.SendMessageBack(tmpFile.Id);
        //    }

        //}

        public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute, IQueueService queueService)
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
                _logger.LogError($"file processed");
                _archiveDocument.Add(new Domain.Models.ArchiveDocument(tmpFile.Id, "none", result.Text));
                _archiveDocument.SaveChangesAsync();

                _tempRepository.Remove(tmpFile);
                _tempRepository.SaveChangesAsync();


                SendMessageBack(tmpFile.Id, queueService);
            }
        }

        public void SendMessageBack(Guid id, IQueueService queueService)
        {
            var msg = new MessageMQ
            {
                Id = id,
                EventName = nameof(ArchiveDocumentProcessedEvent)
            };

            queueService.Send(msg,
                                 "exchangepro.name",
                                 "routing.key");
        }


    }
}
