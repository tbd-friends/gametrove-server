using System;
using System.IO;
using System.Threading.Tasks;
using api.Commands;
using api.Infrastructure;
using api.Query;
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

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetImage(Guid id, ImageSize size = ImageSize.Small)
        {
            var path = await _mediator.Send(new GetFilePathForImage { Id = id });

            return File(path.ResizeImage((int)size), "image/jpeg");
        }
    }
}