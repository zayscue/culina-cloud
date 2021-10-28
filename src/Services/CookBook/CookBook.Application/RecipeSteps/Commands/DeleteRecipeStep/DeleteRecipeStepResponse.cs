using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.DeleteRecipeStep
{
    public class DeleteRecipeStepResponse : IMapFrom<RecipeStep>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
        public string Instruction { get; set; }
    }
}