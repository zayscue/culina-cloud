using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Culina.CookBook.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("cookbook/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
