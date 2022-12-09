using MediatR;

namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public class CreateRecipeDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int EstimatedMinutes { get; set; }
        public string Serves { get; set; } = default!;
        public string Yield { get; set; } = default!;

        public IList<string> Steps { get; set; } = default!;
        public IList<CreateRecipeDtoIngredient> Ingredients { get; set; } = default!;

        public IList<string> ImageUrls { get; set; } = default!;
        public CreateRecipeDtoNutrition Nutrition { get; set; } = default!;
        public IList<CreateRecipeDtoMetadata> Metadata { get; set; } = default!;
        public IList<string> Tags { get; set; } = default!;

        public string CreatedBy { get; set; } = default!;
    }

    public class CreateRecipeDtoMetadata
    {
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
    }

    public class CreateRecipeDtoIngredient
    {
        public string Quantity { get; set; } = default!;
        public string Part { get; set; } = default!;
        public string Type { get; set; } = default!;
    }

    public class CreateRecipeDtoNutrition
    {
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
    }
}
