namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("tags")]
    public class TagsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICookBookService _cookBookService;

        public TagsController(ICurrentUserService currentUserService, ICookBookService cookBookService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
        }

        [HttpGet]
        [Authorize("read:tags")]
        public async Task<ActionResult> GetTags([FromQuery] string name = "", [FromQuery] int page = 1,
            [FromQuery] int limit = 100)
        {
            var tags = await _cookBookService.GetTagsAsync(name, page, limit);
            return Ok(tags);
        }
    }
}
