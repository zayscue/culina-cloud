using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CulinaCloud.CookBook.Application.CookBookStatistics.Queries.GetCookBookStatistics
{
    public class GetCookBookStatisticsResponse : IMapFrom<Statistics>
    {
        public GetRecipeStatisticsResponse RecipeStatistics { get; set; } = new();
        public List<GetRecipeIngredientStatisticsResponse> IngredientStatistics { get; set; } = new();
        public List<GetRecipeTagStatisticsResponse> TagStatistics { get; set; } = new();
    }

    public class GetRecipeStatisticsResponse : IMapFrom<RecipeStatistics>
    {
        public int RecipeCount { get; set; }
    }

    public class GetRecipeIngredientStatisticsResponse : IMapFrom<RecipeIngredientStatistic>
    {
        public int RecipeCount { get; set; }
        public string IngredientName { get; set; }
    }

    public class GetRecipeTagStatisticsResponse : IMapFrom<RecipeTagStatistic>
    {
        public int RecipeCount { get; set; }
        public string TagName { get; set; }
    }
}
