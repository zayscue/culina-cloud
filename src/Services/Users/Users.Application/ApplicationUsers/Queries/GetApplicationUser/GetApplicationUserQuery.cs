using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserQuery : IRequest<GetApplicationUserResponse>
    {
        public string Id { get; set; }
    }

    public class GetApplicationUserQueryHandler : IRequestHandler<GetApplicationUserQuery, GetApplicationUserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetApplicationUserQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetApplicationUserResponse> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Id);
            }

            var response = _mapper.Map<GetApplicationUserResponse>(entity);

            return response;
        }
    }
}