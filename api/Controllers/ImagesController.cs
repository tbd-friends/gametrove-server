using System;
using System.Threading.Tasks;
using api.Infrastructure;
using GameTrove.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GameTrove.Api.Controllers
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

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetImage(Guid id, ImageSize size = ImageSize.Small)
        {
            var path = await _mediator.Send(new GetFilePathForImage { Id = id });

            return File(path.ResizeImage((int)size), "image/jpeg");
        }
    }
}