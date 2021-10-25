using System;
using System.Threading.Tasks;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.CookBook.Application.Tags.Commands.CreateTag;
using CulinaCloud.CookBook.Application.Tags.Queries.GetTag;
using CulinaCloud.CookBook.Application.Tags.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;

namespace CulinaCloud.CookBook.API.Controllers
{
    [Route("tags")]
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