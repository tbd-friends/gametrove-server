using System;
using System.IO;
using System.Threading.Tasks;
using api.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("{id}")]
        public async Task AddImageForGame(Guid id, [FromForm]IFormFile file)
        {
            await _mediator.Send(new AttachImageToGame
            {
                Id = id,
                Content = file.OpenReadStream(),
                FileName = file.FileName
            });
        }
    }
}