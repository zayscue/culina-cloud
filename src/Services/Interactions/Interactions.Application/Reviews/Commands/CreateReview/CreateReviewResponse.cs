using System;
using CulinaCloud.Interactions.Application.Common.Mapping;
using CulinaCloud.Interactions.Domain.Entities;

namespace CulinaCloud.Interactions.Application.Reviews.Commands.CreateReview
{
    public class CreateReviewResponse : IMapFrom<Review>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}