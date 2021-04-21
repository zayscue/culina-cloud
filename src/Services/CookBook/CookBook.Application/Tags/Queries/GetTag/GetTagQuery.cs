using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Culina.CookBook.Application.Common.Exceptions;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Tags.Queries.GetTag
{
    public class GetTagQuery : IRequest<GetTagResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, GetTagResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public GetTagQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GetTagResponse> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Tags
                .AsNoTracking()
                .Where(i => i.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(Tag), request.Id);
            }

            var response = _mapper.Map<GetTagResponse>(entity);

            return response;
        }
    }
}