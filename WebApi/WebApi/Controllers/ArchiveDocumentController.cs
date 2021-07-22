using Domain.Commands;
using Domain.Dtos;
using Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ArchiveDocumentController : ApiControllerBase
    {
        public ArchiveDocumentController(IMediator mediator) : base (mediator) { }
        
        
        [HttpGet("base/{id}")]
        public async Task<ActionResult<ArchiveDocumentDto>> GetDocumentById(Guid id)
        {
            return await QueryAsync(new GetArchiveDocumentQuery(id));
        }

        [HttpGet("fin/{id}")]
        public async Task<ActionResult<ArchiveDocumentDto>> GetFinDocumentById(Guid id)
        {
            return await QueryAsync(new GetFinArchiveDocumentQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult> ProcessDocument([FromForm] IFormFile file)
        {
            byte[] fileBytes = null;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return Ok(await CommandAsync(new ProcessArchiveDocumentCommand(file.FileName, fileBytes)));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateDocument(Guid id, [FromBody] string text)
        {
            return Ok(await CommandAsync(new ArchiveDocumentUpdateCommand(id, text)));
        }
    }
}
