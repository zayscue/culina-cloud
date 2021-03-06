﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.CookBook.Application.Common.Exceptions;
using CulinaCloud.CookBook.Application.Common.Interfaces;
using CulinaCloud.CookBook.Domain.Entities;
using CulinaCloud.CookBook.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<CreateTagResponse>
    {
        public Guid? Id { get; set; }
        public string TagName { get; set; }
    }

    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, CreateTagResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IAggregateEventService _aggregateEventService;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IAggregateEventService aggregateEventService,
            IMapper mapper
            )
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _aggregateEventService = aggregateEventService;
            _mapper = mapper;
        }

        public async Task<CreateTagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTime.Now;
            var currentUserId = _currentUserService.UserId;
            var entity = new Tag
            {
                Id = request.Id ?? Guid.NewGuid(),
                TagName = request.TagName,
                Created = now,
                CreatedBy = currentUserId
            };
            var @event = new TagCreatedEvent
            {
                AggregateId = entity.Id,
                Details = "A new recipe tag was created using the POST \"/cookbook/tags\" API.",
                Occurred = entity.Created,
                RaisedBy = entity.CreatedBy,
                Data = { Id = entity.Id, TagName = entity.TagName }
            };
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                await _context.EventOutbox.AddAsync(new AggregateEventEntity(@event.ToAggregateEvent()),
                    cancellationToken);
                await _context.Tags.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _aggregateEventService.Publish(@event, cancellationToken);

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