using System;
using System.Threading.Tasks;
using GameTrove.Api.Models;
using GameTrove.Application.Commands;
using GameTrove.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("titles")]
    public class TitleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TitleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}"), Authorize(Roles = "Administrator,User")]
        public async Task<ActionResult<TitleViewModel>> UpdateTitle(Guid id, UpdateTitleModel model)
        {
            var result = await _mediator.Send(new UpdateTitle
            {
                TitleId = id,
                Name = model.Name,
                Subtitle = model.Subtitle,
                Genres = model.Genres
            });

            return result != null ? new ActionResult<TitleViewModel>(result) : BadRequest();
        }
    }
}