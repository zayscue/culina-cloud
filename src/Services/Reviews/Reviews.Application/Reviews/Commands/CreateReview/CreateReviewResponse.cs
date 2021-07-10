using System;
using CulinaCloud.Reviews.Domain.Entities;
using CulinaCloud.Reviews.Application.Common.Mapping;

namespace CulinaCloud.Reviews.Application.Reviews.Commands.CreateReview
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