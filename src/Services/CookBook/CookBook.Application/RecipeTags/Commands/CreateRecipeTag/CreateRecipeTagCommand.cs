using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.CreateRecipeTag
{
    public class CreateRecipeTagCommand : IRequest<CreateRecipeTagResponse>
    {
        public Guid RecipeId { get; set; }
        public string TagName { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRecipeTagCommandHandler : IRequestHandler<CreateRecipeTagCommand, CreateRecipeTagResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateRecipeTagCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CreateRecipeTagResponse> Handle(CreateRecipeTagCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .SingleOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken);
            if (recipe == null)
            {
                throw new NotFoundException(nameof(Recipe), request.RecipeId);
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(x => x.TagName.ToLower().Equals(request.TagName.ToLower()), cancellationToken);
            if (tag == null)
            {
                tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    TagName = request.TagName,
                    CreatedBy = request.CreatedBy
                };
                await _context.Tags.AddAsync(tag, cancellationToken);
            }

            var entity = new RecipeTag
            {
                RecipeId = request.RecipeId,
                Recipe = recipe,
                TagId = tag.Id,
                Tag = tag,
                CreatedBy = request.CreatedBy
            };

            await _context.RecipeTags.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateRecipeTagResponse>(entity);

            return response;
        }
    }
}