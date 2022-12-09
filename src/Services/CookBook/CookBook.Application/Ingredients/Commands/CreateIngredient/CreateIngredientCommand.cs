using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand : IRequest<CreateIngredientResponse>
    {
        public Guid? Id { get; set; }
        public string IngredientName { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, CreateIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateIngredientCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateIngredientResponse> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var entity = new Ingredient
            {
                Id = request.Id ?? Guid.NewGuid(),
                IngredientName = request.IngredientName,
                CreatedBy = request.CreatedBy
            };
            try
            {
                await _context.Ingredients.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateIngredientResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("IX_Ingredients_IngredientName", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(Ingredient), entity.IngredientName);
                }

                throw;
            }
        }
    }
}
