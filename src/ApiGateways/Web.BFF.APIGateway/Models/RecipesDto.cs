﻿namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record RecipesDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int EstimatedMinutes { get; set; }
    public string? Serves { get; set; }
    public string? Yield { get; set; }
    public int NumberOfSteps { get; set; }
    public int NumberOfIngredients { get; set; }
    
    public IList<RecipeImage>? Images { get; set; }
}

public record RecipeImage
{
    public Guid RecipeId { get; set; }
    public Guid ImageId { get; set; }
    public string? Url { get; set; }
}