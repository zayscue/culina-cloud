using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.RecipeTags.Commands.DeleteRecipeTag
{
    public class DeleteRecipeTagCommand : IRequest<DeleteRecipeTagResponse>
    {
        public Guid RecipeId { get; set; }
        public Guid TagId { get; set; }
    }

    public class DeleteRecipeTagCommandHandler : IRequestHandler<DeleteRecipeTagCommand, DeleteRecipeTagResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipeTagCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DeleteRecipeTagResponse> Handle(DeleteRecipeTagCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeTags
                .AsNoTracking()
                .Include(x => x.Tag)
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId
                    && x.TagId == request.TagId, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeTag),
                    JsonSerializer.Serialize(new
                    {
                        RecipeId = request.RecipeId,
                        TagId = request.TagId
                    })
                );
            }

            _context.RecipeTags.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<DeleteRecipeTagResponse>(entity);

            return response;
        }
    }
}