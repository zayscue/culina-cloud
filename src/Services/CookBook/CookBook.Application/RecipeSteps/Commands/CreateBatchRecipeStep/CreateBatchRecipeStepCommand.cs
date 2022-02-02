using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.RecipeSteps.Commands.CreateRecipeStep;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.CreateBatchRecipeStep
{
    public class CreateBatchRecipeStepCommand : IRequest<List<CreateRecipeStepResponse>>
    {
        public Guid RecipeId { get; set; }
        public List<CreateRecipeStepCommand> Commands { get; set; }
    }

    public class CreateBatchRecipeStepCommandHandler
        : IRequestHandler<CreateBatchRecipeStepCommand, List<CreateRecipeStepResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateBatchRecipeStepCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<List<CreateRecipeStepResponse>> Handle(CreateBatchRecipeStepCommand request, 
            CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Steps)
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }
            var commands = request.Commands;
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _context.RecipeSteps.RemoveRange(recipe.Steps);
            await _context.SaveChangesAsync(cancellationToken);
            var order = 1;
            var entities = new List<RecipeStep>();
            foreach (var command in commands)
            {
                var entity = new RecipeStep
                {
                    RecipeId = request.RecipeId,
                    Order = order,
                    Instruction = command.Instruction,
                    CreatedBy = command.CreatedBy,
                    Recipe = recipe
                };
                await _context.RecipeSteps.AddAsync(entity, cancellationToken);
                entities.Add(entity);                
                order += 1;
            }
            recipe.NumberOfSteps = commands.Count;
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var response = await entities.AsQueryable()
                .ProjectTo<CreateRecipeStepResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}