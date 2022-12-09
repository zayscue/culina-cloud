using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.CreateRecipeStep
{
    public class CreateRecipeStepCommand : IRequest<CreateRecipeStepResponse>
    {
        public Guid RecipeId { get; set; }
        public string Instruction { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRecipeStepCommandHandler : IRequestHandler<CreateRecipeStepCommand, CreateRecipeStepResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeStepCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CreateRecipeStepResponse> Handle(CreateRecipeStepCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Steps)
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }
            var numberOfSteps = recipe.NumberOfSteps;
            numberOfSteps += 1;
            var entity = new RecipeStep
            {
                RecipeId = request.RecipeId,
                Order = numberOfSteps,
                Instruction = request.Instruction,
                CreatedBy = request.CreatedBy,
                Recipe = recipe
            };
            recipe.NumberOfSteps = numberOfSteps;
            _context.Recipes.Update(recipe);
            await _context.RecipeSteps.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<CreateRecipeStepResponse>(entity);
            return response;
        }
    }
}