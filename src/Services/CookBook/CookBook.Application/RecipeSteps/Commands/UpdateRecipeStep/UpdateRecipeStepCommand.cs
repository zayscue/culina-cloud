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

namespace CulinaCloud.CookBook.Application.RecipeSteps.Commands.UpdateRecipeStep
{
    public class UpdateRecipeStepCommand : IRequest<UpdateRecipeStepResponse>
    {
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
        public string Instruction { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class UpdateRecipeStepCommandHandler : IRequestHandler<UpdateRecipeStepCommand, UpdateRecipeStepResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateRecipeStepCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UpdateRecipeStepResponse> Handle(UpdateRecipeStepCommand request, CancellationToken cancellationToken)
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
            entity.Instruction = request.Instruction;
            entity.LastModifiedBy = request.LastModifiedBy;

            _context.RecipeSteps.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<UpdateRecipeStepResponse>(entity);
            return response;
        }
    }
}