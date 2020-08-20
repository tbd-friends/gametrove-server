using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Storage.Blobs;
using GameTrove.Application.Infrastructure;
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
            var image = await _mediator.Send(new GetSizedImageById { Id = id, Size = size });

            if (image.Length > 0)
            {
                return File(image, "image/jpeg");
            }

            return new EmptyResult();
        }
    }
}