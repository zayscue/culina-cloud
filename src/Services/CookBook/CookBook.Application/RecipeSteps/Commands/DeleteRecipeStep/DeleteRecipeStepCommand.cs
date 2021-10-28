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

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.DeleteRecipeStep
{
    public class DeleteRecipeStepCommand : IRequest<DeleteRecipeStepResponse>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
    }

    public class DeleteRecipeStepCommandHandler : IRequestHandler<DeleteRecipeStepCommand, DeleteRecipeStepResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRecipeStepCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DeleteRecipeStepResponse> Handle(DeleteRecipeStepCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeSteps
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == request.RecipeId && x.Order == request.Order, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(RecipeStep),
                    JsonSerializer.Serialize(new { RecipeId = request.RecipeId, Order = request.Order })
                );
            }
            _context.RecipeSteps.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<DeleteRecipeStepResponse>(entity);
            return response;
        }
    }
}