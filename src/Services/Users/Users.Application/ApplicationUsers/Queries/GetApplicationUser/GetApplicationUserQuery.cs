using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.Users.Application.Interfaces;
using MediatR;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserQuery : IRequest<GetApplicationUserResponse>
    {
        public string Id { get; set; }
    }

    public class GetApplicationUserQueryHandler : IRequestHandler<GetApplicationUserQuery, GetApplicationUserResponse>
    {
        private readonly IApplicationUserManagementService _users;
        private readonly IMapper _mapper;

        public GetApplicationUserQueryHandler(
            IApplicationUserManagementService users,
            IMapper mapper)
        {
            _users = users;
            _mapper = mapper;
        }

        public async Task<GetApplicationUserResponse> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _users.GetApplicationUser(request.Id, cancellationToken);

            var response = _mapper.Map<GetApplicationUserResponse>(entity);

            return response;
        }
    }
}