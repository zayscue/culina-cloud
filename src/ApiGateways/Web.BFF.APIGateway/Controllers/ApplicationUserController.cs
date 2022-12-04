namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("users")]
    [Authorize]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUsersService _usersService;

        public ApplicationUserController(ICurrentUserService currentUserService, IUsersService usersService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpGet("me")]
        [Authorize("read:me")]
        public async Task<ActionResult> Me()
        {
            var userId = _currentUserService.UserId;
            var applicationUser = await _usersService.GetApplicationUserAsync(userId);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return Ok(applicationUser);
        }
    }
}
