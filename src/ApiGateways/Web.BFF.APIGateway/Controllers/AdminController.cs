namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICookBookService _cookBookService;
        private readonly IUsersService _usersService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IInteractionsService _interactionsService;

        public AdminController(
            ICurrentUserService currentUserService,
            ICookBookService cookBookService,
            IUsersService usersService,
            IAnalyticsService analyticsService,
            IInteractionsService interactionsService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
            _interactionsService = interactionsService ?? throw new ArgumentNullException(nameof(interactionsService));
        }

        [HttpGet("statistics")]
        public async Task<ActionResult> GetApplicationStatistics()
        {

            return Ok();
        }
    }
}
