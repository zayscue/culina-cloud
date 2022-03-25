using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Users.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommand : IRequest<CreateApplicationUserResponse>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand, CreateApplicationUserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateApplicationUserCommandHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateApplicationUserResponse> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new ApplicationUser
            {
                Id = request.Id,
                Email = request.Email,
                DisplayName = request.DisplayName,
                Picture = request.Picture,
                CreatedBy = request.CreatedBy
            };
            try
            {
                await _context.ApplicationUsers.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var response = _mapper.Map<CreateApplicationUserResponse>(entity);
                return response;
            }
            catch (DbUpdateException e)
            {
               if (e.InnerException?.Message.Contains("PK_ApplicationUsers", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(ApplicationUser), entity.Id);
                }
                throw;
            }
        }
    }
}