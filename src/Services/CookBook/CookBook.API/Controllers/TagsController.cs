using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Culina.CookBook.Application.Common.Models;
using Culina.CookBook.Application.Tags.Commands.CreateTag;
using Culina.CookBook.Application.Tags.Queries.GetTag;
using Culina.CookBook.Application.Tags.Queries.GetTags;

namespace Culina.CookBook.API.Controllers
{
    public class TagsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetTagsResponse>>> Get([FromQuery] GetTagsQuery query)
        {
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetTagResponse>> Get(Guid id)
        {
            var vm = await Mediator.Send(new GetTagQuery() {Id = id});
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<CreateTagResponse>> Create(CreateTagCommand command)
        {
            var vm = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id = vm.Id}, vm);
        }
    }
}