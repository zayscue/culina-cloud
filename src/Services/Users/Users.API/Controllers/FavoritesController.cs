﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CulinaCloud.BuildingBlocks.API.Controllers;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using CulinaCloud.Users.Application.Favorites.Commands.CreateFavorite;
using CulinaCloud.Users.Application.Favorites.Commands.DeleteFavorite;
using CulinaCloud.Users.Application.Favorites.Queries.GetFavorites;
using Microsoft.AspNetCore.Authorization;

namespace CulinaCloud.Users.API.Controllers
{
    [Authorize]
    [Route("users/favorites")]
    public class FavoritesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetFavoritesResponse>>> Get([FromQuery] GetFavoritesQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateFavoriteResponse>> Create([FromBody] CreateFavoriteCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<DeleteFavoriteResponse>> Delete([FromBody] DeleteFavoriteCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
