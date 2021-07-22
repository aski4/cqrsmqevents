using System;

namespace Domain.Models
{
    public class TempFile : BaseModel
    {
        public Guid Id { get; set; }
        public byte[] FileBytes { get; set; }

        public TempFile( byte[] fileBytes)
        {
            Id = Guid.NewGuid();
            FileBytes = fileBytes;
        }
    }
}
