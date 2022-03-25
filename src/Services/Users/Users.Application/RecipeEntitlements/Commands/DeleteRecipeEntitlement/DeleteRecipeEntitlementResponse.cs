using System;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.DeleteRecipeEntitlement
{
    public class DeleteRecipeEntitlementResponse : IMapFrom<RecipeEntitlement>
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set;}
        public string UserId { get; set; }
        public string Type { get; set;}
        public string GrantedBy { get; set; }
        public DateTime Granted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RecipeEntitlement, DeleteRecipeEntitlementResponse>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type.ToString().Trim().ToLower()))
                .ForMember(d => d.GrantedBy, opt => opt.MapFrom(src => src.LastModifiedBy ?? src.CreatedBy))
                .ForMember(d => d.Granted, opt => opt.MapFrom(src => src.LastModified.HasValue ? src.LastModified.Value : src.Created));
        }
    }
}