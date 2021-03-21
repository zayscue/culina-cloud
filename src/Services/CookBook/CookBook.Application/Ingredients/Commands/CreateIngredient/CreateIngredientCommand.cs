using System;
using System.Threading;
using System.Threading.Tasks;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;
using MediatR;

namespace Culina.CookBook.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }
        public string IngredientName { get; set; }
    }

    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateIngredientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var entity = new Ingredient()
            {
                Id = request.Id ?? Guid.NewGuid(),
                IngredientName = request.IngredientName
            };

            await _context.Ingredients.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
