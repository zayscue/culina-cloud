namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("ingredients")]
    public class IngredientsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICookBookService _cookBookService;

        public IngredientsController(ICurrentUserService currentUserService, ICookBookService cookBookService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _cookBookService = cookBookService ?? throw new ArgumentNullException(nameof(cookBookService));
        }

        [HttpGet]
        [Authorize("read:ingredients")]
        public async Task<ActionResult> GetIngredients([FromQuery] string name = "", [FromQuery] int page = 1,
            [FromQuery] int limit = 100)
        {
            var ingredients = await _cookBookService.GetIngredientsAsync(name, page, limit);
            return Ok(ingredients);
        }
    }
}
