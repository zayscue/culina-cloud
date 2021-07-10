using System;
using CulinaCloud.BuildingBlocks.Common;

namespace CulinaCloud.Reviews.Domain.Entities
{
    public class Review : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}