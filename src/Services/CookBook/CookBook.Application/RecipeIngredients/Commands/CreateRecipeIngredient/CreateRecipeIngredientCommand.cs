using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeIngredients.Commands.CreateRecipeIngredient
{
    public class CreateRecipeIngredientCommand : IRequest<CreateRecipeIngredientResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid? RecipeIngredientId { get; set; }
        public string Quantity { get; set; }
        public string Part { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRecipeIngredientCommandHandler : IRequestHandler<CreateRecipeIngredientCommand, CreateRecipeIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeIngredientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CreateRecipeIngredientResponse> Handle(CreateRecipeIngredientCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(x => x.IngredientName.ToLower().Equals(request.Type.ToLower()), cancellationToken);
            Guid? ingredientId = ingredient != null ? ingredient.Id : null;
            var entity = new RecipeIngredient
            {
                Id = request.RecipeIngredientId ?? Guid.NewGuid(),
                RecipeId = request.RecipeId,
                Recipe = recipe,
                IngredientId = ingredientId,
                Ingredient = ingredient,
                Quantity = request.Quantity,
                Part = request.Part,
                CreatedBy = request.CreatedBy
            };

            await _context.RecipeIngredients.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateRecipeIngredientResponse>(entity);

            return response;
        }
    }
}