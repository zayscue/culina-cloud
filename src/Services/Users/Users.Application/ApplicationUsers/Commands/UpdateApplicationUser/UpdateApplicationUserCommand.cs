using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;

namespace CulinaCloud.Users.Application.ApplicationUsers.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommand : IRequest<UpdateApplicationUserResponse>
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class UpdateApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand, UpdateApplicationUserResponse>
    {
        private readonly IApplicationUserManagementService _users;
        private readonly IMapper _mapper;

        public UpdateApplicationUserCommandHandler(
            IApplicationUserManagementService users,
            IMapper mapper)
        {
            _users = users;
            _mapper = mapper;
        }

        public async Task<UpdateApplicationUserResponse> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var updatedApplicationUser = new ApplicationUser
            {
                Id = request.Id,
                DisplayName = request.DisplayName,
                Picture = request.Picture,
                LastModifiedBy = request.LastModifiedBy
            };
            var entity = await _users.SaveApplicationUser(updatedApplicationUser, cancellationToken);
            var response = _mapper.Map<UpdateApplicationUserResponse>(entity);
            return response;
        }
    }
}