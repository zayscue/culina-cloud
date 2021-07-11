using System;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Interactions.Domain.Entities;

namespace CulinaCloud.Interactions.Application.Reviews.Queries.GetReviews
{
    public class GetReviewsResponse : IMapFrom<Review>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}
