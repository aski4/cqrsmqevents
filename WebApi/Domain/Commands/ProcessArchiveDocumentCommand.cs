using Domain.Dtos;

namespace Domain.Commands
{
    public class ProcessArchiveDocumentCommand : CommandBase<VoidClassDto>
    {
        public ProcessArchiveDocumentCommand()
        {

        }

        public ProcessArchiveDocumentCommand(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }

        public string FileName { get; }
        public byte[] FileBytes { get; }
    }
}
