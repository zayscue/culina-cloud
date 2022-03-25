using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using CulinaCloud.Users.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.RecipeEntitlements.Commands.CreateRecipeEntitlement
{
    public class CreateRecipeEntitlementCommand : IRequest<CreateRecipeEntitlementResponse>
    {
        public Guid? Id { get; set; }
        public string UserId { get; set; }
        public Guid RecipeId { get; set; }
        public string Type { get; set; }
        public string GrantedBy { get; set; }
    }

    public class CreateRecipeEntitlementCommandHandler : IRequestHandler<CreateRecipeEntitlementCommand, CreateRecipeEntitlementResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeEntitlementCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateRecipeEntitlementResponse> Handle(CreateRecipeEntitlementCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id ?? Guid.NewGuid();
            var entity = new RecipeEntitlement
            {
                Id = id,
                UserId = request.UserId,
                RecipeId = request.RecipeId,
                CreatedBy = request.GrantedBy,
                Type = (RecipeEntitlementType)Enum.Parse(typeof(RecipeEntitlementType), request.Type.Trim().ToUpper())
            };
            try
            {
                var recipeAuthor = await _context.RecipeEntitlements
                    .SingleOrDefaultAsync(x => x.RecipeId == entity.RecipeId && x.Type == RecipeEntitlementType.AUTHOR, cancellationToken);

                if (recipeAuthor != null && entity.Type == RecipeEntitlementType.AUTHOR)
                {
                    throw new CanNotHaveMoreThanOneAuthorException(entity.RecipeId);
                }

                if (recipeAuthor != null && !string.Equals(recipeAuthor.UserId, entity.CreatedBy, StringComparison.OrdinalIgnoreCase))
                {
                    throw new CanNotModifyRecipeEntitlementException(entity.RecipeId, entity.UserId);
                }

                if (recipeAuthor == null && entity.Type != RecipeEntitlementType.AUTHOR)
                {
                    throw new RecipeHasNoAuthorException(entity.RecipeId);
                }

                if (recipeAuthor == null && entity.Type == RecipeEntitlementType.AUTHOR && !string.Equals(entity.CreatedBy, entity.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new RecipeAuthorEntitlementDoesNotMatch(entity.UserId, entity.CreatedBy);
                }

                await _context.RecipeEntitlements.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateRecipeEntitlementResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
               if (e.InnerException?.Message.Contains("PK_RecipeEntitlements", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(RecipeEntitlement), id.ToString());
                }

                if (e.InnerException?.Message.Contains("IX_RecipeEntitlements_UserId_RecipeId", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(RecipeEntitlement), JsonSerializer.Serialize(new {
                        RecipeId = entity.RecipeId.ToString(),
                        UserId = entity.UserId
                    }));
                }

                throw;
            }
        }
    }
}