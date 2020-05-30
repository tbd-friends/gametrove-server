using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Api.Models;
using GameTrove.Application.Commands;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
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
        public async Task<Guid> RegisterCopy(Guid id, RegisterCopyModel model)
        {
            return await _mediator.Send(new RegisterCopy
            {
                GameId = id,
                Tags = model.Tags,
                Cost = model.Cost,
                Purchased = model.Purchased
            });
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<CopyViewModel>> GetCopies(Guid id)
        {
            return await _mediator.Send(new GetCopies
            {
                GameId = id
            });
        }
    }
}