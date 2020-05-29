using System;
using System.Threading.Tasks;
using GameTrove.Api.Models;
using GameTrove.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("copies")]
    public class CopyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CopyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}")]
        public async Task<bool> RegisterCopy(Guid id, RegisterCopyModel model)
        {
            await _mediator.Send(new RegisterCopy
            {
                GameId = id,
                Tags = model.Tags,
                Cost = model.Cost
            });

            return true;
        }
    }
}