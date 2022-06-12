using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.ApplicationUsersPolicies.Queries.GetApplicationUsersPolicies
{
    public class GetApplicationUsersPoliciesQuery : IRequest<List<GetApplicationUsersPoliciesResponse>> 
    {
        public Guid? RecipeId { get; set; }
        public string UserId { get; set; }
        public List<Guid> RecipeIds { get; set; } = new List<Guid>();
    }

    public class GetApplicationUsersRecipePoliciesQueryHandler : IRequestHandler<GetApplicationUsersPoliciesQuery, List<GetApplicationUsersPoliciesResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetApplicationUsersRecipePoliciesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetApplicationUsersPoliciesResponse>> Handle(GetApplicationUsersPoliciesQuery request, CancellationToken cancellationToken)
        {
            var recipesPoliciesDict = new Dictionary<Guid, GetApplicationUsersPoliciesResponse>();
            if (request.RecipeId.HasValue && request.RecipeId.Value != Guid.Empty)
            {
                if (!recipesPoliciesDict.ContainsKey(request.RecipeId.Value))
                {
                    recipesPoliciesDict.Add(request.RecipeId.Value, new GetApplicationUsersPoliciesResponse
                    {
                        RecipeId = request.RecipeId.Value,
                        UserId = request.UserId,
                        IsAFavorite = false,
                        IsOwner = false,
                        CanEdit = false,
                        CanShare = false
                    });
                }
            }
            foreach (var recipeId in request.RecipeIds)
            {
                if (!recipesPoliciesDict.ContainsKey(recipeId))
                {
                    recipesPoliciesDict.Add(recipeId, new GetApplicationUsersPoliciesResponse
                    {
                        RecipeId = recipeId,
                        UserId = request.UserId,
                        IsAFavorite = false,
                        IsOwner = false,
                        CanEdit = false,
                        CanShare = false
                    });
                }
            }

            try
            {
                var usersFavoriteRecipes = await _context.Favorites
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .WhereIf(request.RecipeIds.Count > 0,
                        x => request.RecipeIds.Contains(x.RecipeId))
                    .WhereIf(request.RecipeId.HasValue && request.RecipeId != Guid.Empty,
                        x => x.RecipeId == request.RecipeId.Value)
                    .ToListAsync(cancellationToken);
                var usersRecipeEntitlements = await _context.RecipeEntitlements
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .WhereIf(request.RecipeIds.Count > 0,
                        x => request.RecipeIds.Contains(x.RecipeId))
                    .WhereIf(request.RecipeId.HasValue && request.RecipeId != Guid.Empty,
                        x => x.RecipeId == request.RecipeId.Value)
                    .ToListAsync(cancellationToken);

                foreach(var usersFavoriteRecipe in usersFavoriteRecipes)
                {
                    if (recipesPoliciesDict.ContainsKey(usersFavoriteRecipe.RecipeId))
                    {
                        var recipePolicy = recipesPoliciesDict[usersFavoriteRecipe.RecipeId];
                        recipePolicy.IsAFavorite = true;
                    }
                    else
                    {
                        recipesPoliciesDict.Add(usersFavoriteRecipe.RecipeId, new GetApplicationUsersPoliciesResponse
                        {
                            RecipeId = usersFavoriteRecipe.RecipeId,
                            UserId = request.UserId,
                            IsAFavorite = true,
                            IsOwner = false,
                            CanEdit = false,
                            CanShare = false
                        });
                    }
                }
                foreach (var usersRecipeEntitlement in usersRecipeEntitlements)
                {
                    if (recipesPoliciesDict.ContainsKey(usersRecipeEntitlement.RecipeId))
                    {
                        var recipePolicy = recipesPoliciesDict[usersRecipeEntitlement.RecipeId];
                        recipePolicy.CanEdit = usersRecipeEntitlement.Type == RecipeEntitlementType.CONTRIBUTOR
                            || usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR;
                        recipePolicy.IsOwner = usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR;
                        recipePolicy.CanShare = usersRecipeEntitlement.Type == RecipeEntitlementType.CONTRIBUTOR
                            || usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR
                            || usersRecipeEntitlement.Type == RecipeEntitlementType.READER;
                    }
                    else
                    {
                        recipesPoliciesDict.Add(usersRecipeEntitlement.RecipeId, new GetApplicationUsersPoliciesResponse
                        {
                            RecipeId = usersRecipeEntitlement.RecipeId,
                            UserId = request.UserId,
                            IsAFavorite = false,
                            IsOwner = usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR,
                            CanEdit = usersRecipeEntitlement.Type == RecipeEntitlementType.CONTRIBUTOR
                                || usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR,
                            CanShare = usersRecipeEntitlement.Type == RecipeEntitlementType.CONTRIBUTOR
                                || usersRecipeEntitlement.Type == RecipeEntitlementType.AUTHOR
                                || usersRecipeEntitlement.Type == RecipeEntitlementType.READER
                        });
                    }
                }
            }
            catch { }

            var response = recipesPoliciesDict.Values.ToList();
            return response;
        }
    }
}
