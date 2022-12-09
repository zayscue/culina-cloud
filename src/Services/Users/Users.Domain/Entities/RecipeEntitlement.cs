using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.Users.Domain.Enums;
using System;

namespace CulinaCloud.Users.Domain.Entities
{
    public class RecipeEntitlement : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set;}
        public string UserId { get; set; }
        public RecipeEntitlementType Type { get; set;}
    }
}