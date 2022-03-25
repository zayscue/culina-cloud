using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateApplicationUserCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateApplicationUserResponse> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(
                    nameof(ApplicationUser),
                    request.Id
                );
            }

            if (!string.IsNullOrWhiteSpace(request.DisplayName))
            {
                entity.DisplayName = request.DisplayName;
            }
            if (!string.IsNullOrWhiteSpace(request.Picture))
            {
                entity.Picture = request.Picture;
            }
            entity.LastModifiedBy = request.LastModifiedBy;

            _context.ApplicationUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<UpdateApplicationUserResponse>(entity);
            return response;
        }
    }
}