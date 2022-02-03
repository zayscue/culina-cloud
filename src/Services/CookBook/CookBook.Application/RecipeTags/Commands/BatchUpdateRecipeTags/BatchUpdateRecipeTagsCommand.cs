using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Application.RecipeTags.Commands.CreateRecipeTag;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.BatchUpdateRecipeTags
{
    public class BatchUpdateRecipeTagsCommand : IRequest<List<CreateRecipeTagResponse>>
    {
        public Guid RecipeId { get; set; }
        public List<CreateRecipeTagCommand> Commands { get; set; }
    }

    public class BatchUpdateRecipeTagsCommandHandler :
        IRequestHandler<BatchUpdateRecipeTagsCommand, List<CreateRecipeTagResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BatchUpdateRecipeTagsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<List<CreateRecipeTagResponse>> Handle(BatchUpdateRecipeTagsCommand request, 
            CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }

            var existingRecipeTags = recipe.Tags.ToList();
            var commands = request.Commands;
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _context.RecipeTags.RemoveRange(existingRecipeTags);
            await _context.SaveChangesAsync(cancellationToken);
            var recipeTags = new List<RecipeTag>();
            foreach (var command in commands)
            {
                var tag = await _context.Tags
                    .AsNoTracking()
                    .SingleOrDefaultAsync(
                        x => x.TagName.ToLower().Equals(command.TagName.Trim().ToLower()),
                        cancellationToken);
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Id = Guid.NewGuid(),
                        TagName = command.TagName.Trim(),
                        CreatedBy = command.CreatedBy
                    };
                    await _context.Tags.AddAsync(tag, cancellationToken);
                }

                var entity = new RecipeTag
                {
                    RecipeId = request.RecipeId,
                    TagId = tag.Id,
                    CreatedBy = command.CreatedBy
                };
                recipeTags.Add(entity);
            }
            await _context.RecipeTags.AddRangeAsync(recipeTags, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            var updatedRecipeTagsQuery = _context.RecipeTags
                .AsNoTracking()
                .Include(x => x.Tag)
                .Where(x => x.RecipeId == request.RecipeId);

            var results = await updatedRecipeTagsQuery
                .ProjectToListAsync<CreateRecipeTagResponse>(_mapper.ConfigurationProvider, cancellationToken);

            return results;
        }
    }
}