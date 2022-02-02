namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int EstimatedMinutes { get; set; }
    public string Serves { get; set; } = default!;
    public string Yield { get; set; } = default!;
    public int NumberOfSteps { get; set; }
    public int NumberOfIngredients { get; set; }
    public string? LastModifiedBy { get; set; }

    public IList<RecipeStepDto>? Steps { get; set; } = default!;
    public IList<RecipeIngredientDto> Ingredients { get; set; } = default!;
    public IList<RecipeImageDto> Images { get; set; } = default!;
    public RecipeNutritionDto? Nutrition { get; set; } = default!;
    public IList<RecipeTagDto> Tags { get; set; } = default!;
}

public record RecipeStepDto
{
    public Guid RecipeId { get; set; }
    public int Order { get; set; }
    public string Instruction { get; set; } = default!;
    public string? CreatedBy { get; set; }
}

public record RecipeIngredientDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid? IngredientId { get; set; }
    public string? Quantity { get; set; }
    public string? Part { get; set; }
    public string IngredientName { get; set; } = default!;
}

public record RecipeImageDto
{
    public Guid RecipeId { get; set; }
    public Guid ImageId { get; set; }
    public string Url { get; set; } = default!;
}

public record RecipeTagDto
{
    public Guid RecipeId { get; set; }
    public Guid TagId { get; set; }
    public string TagName { get; set; } = default!;
}

public record RecipeNutritionDto
{
    public Guid RecipeId { get; set; }
    public string ServingSize { get; set; } = default!;
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

    public string? LastModifiedBy { get; set; }
}

