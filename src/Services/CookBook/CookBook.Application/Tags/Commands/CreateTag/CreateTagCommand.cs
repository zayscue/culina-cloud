using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Culina.CookBook.Application.Common.Exceptions;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Culina.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<CreateTagResponse>
    {
        public Guid? Id { get; set; }
        public string TagName { get; set; }
    }

    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, CreateTagResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<CreateTagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var entity = new Tag()
            {
                Id = request.Id ?? Guid.NewGuid(),
                TagName = request.TagName
            };
            try
            {
                await _context.Tags.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CreateTagResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message?.Contains("IX_Tags_TagName", StringComparison.Ordinal) ?? false)
                {
                    throw new EntityConflictException(nameof(Tag), entity.TagName);
                }
                throw;
            }
        }
    }
}