using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Application.Common.Exceptions;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Ingredients.Queries.GetIngredient
{
    public class GetIngredientQuery : IRequest<GetIngredientResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetIngredientQueryHandler : IRequestHandler<GetIngredientQuery, GetIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public GetIngredientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GetIngredientResponse> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Ingredients
                .AsNoTracking()
                .Where(i => i.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Ingredient), request.Id);
            }

            var response = _mapper.Map<GetIngredientResponse>(entity);

            return response;
        }
    }
}