namespace CulinaCloud.Web.BFF.APIGateway.Models;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int EstimatedMinutes { get; set; }
    public string Serves { get; set; }
    public string Yield { get; set; }
    public int NumberOfSteps { get; set; }
    public int NumberOfIngredients { get; set; }

    public IList<RecipeStepDto> Steps { get; set; }
    public IList<RecipeIngredientDto> Ingredients { get; set; }
    public IList<RecipeImageDto> Images { get; set; }
    public RecipeNutritionDto Nutrition { get; set; }
    public IList<RecipeTagDto> Tags { get; set; }
}

public class RecipeStepDto
{
    public Guid RecipeId { get; set; }
    public int Order { get; set; }
    public string Instruction { get; set; }
}

public class RecipeIngredientDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid? IngredientId { get; set; }
    public string Quantity { get; set; }
    public string Part { get; set; }
    public string IngredientName { get; set; }
}

public class RecipeImageDto
{
    public Guid RecipeId { get; set; }
    public Guid ImageId { get; set; }
    public string Url { get; set; }
}

public class RecipeTagDto
{
    public Guid RecipeId { get; set; }
    public Guid TagId { get; set; }
    public string TagName { get; set; }
}

public class RecipeNutritionDto
{
    public Guid RecipeId { get; set; }
    public string ServingSize { get; set; }
    public int ServingsPerRecipe { get; set; }
    public decimal Calories { get; set; }
    public decimal CaloriesFromFat { get; set; }
    public decimal CaloriesFromFatPdv { get; set; }
    public decimal TotalFat { get; set; }
    public decimal TotalFatPdv { get; set; }
    public decimal SaturatedFat { get; set; }
    public decimal SaturatedFatPdv { get; set; }
    public decimal Cholesterol { get; set; }
    public decimal CholesterolPdv { get; set; }
    public decimal DietaryFiber { get; set; }
    public decimal DietaryFiberPdv { get; set; }
    public decimal Sugar { get; set; }
    public decimal SugarPdv { get; set; }
    public decimal Sodium { get; set; }
    public decimal SodiumPdv { get; set; }
    public decimal Protein { get; set; }
    public decimal ProteinPdv { get; set; }
    public decimal TotalCarbohydrates { get; set; }
    public decimal TotalCarbohydratesPdv { get; set; }
}

