using CulinaCloud.BuildingBlocks.Common;
using System;

namespace CulinaCloud.Users.Domain.Entities
{
    public class Favorite : AuditableEntity
    {
        public Guid RecipeId {  get; set; }
        public string UserId { get; set; }
    }
}
