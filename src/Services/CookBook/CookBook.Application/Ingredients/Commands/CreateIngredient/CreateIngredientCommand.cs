using System;
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

namespace CulinaCloud.CookBook.Application.Ingredients.Commands.CreateIngredient
{
  public class CreateIngredientCommand : IRequest<CreateIngredientResponse>
    {
        public Guid? Id { get; set; }
        public string IngredientName { get; set; }
    }

    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, CreateIngredientResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IAggregateEventService _aggregateEventService;
        private readonly IMapper _mapper;

        public CreateIngredientCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IAggregateEventService aggregateEventService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _aggregateEventService = aggregateEventService;
            _mapper = mapper;
        }

        public async Task<CreateIngredientResponse> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTime.Now;
            var currentUserId = _currentUserService.UserId;
            var entity = new Ingredient
            {
                Id = request.Id ?? Guid.NewGuid(),
                IngredientName = request.IngredientName,
                Created = now,
                CreatedBy = currentUserId
            };
            var @event = new IngredientCreatedEvent
            {
                AggregateId = entity.Id,
                Details = "A new ingredient was created using the POST \"/cookbook/ingredients\" API.",
                Occurred = entity.Created,
                RaisedBy = entity.CreatedBy,
                Data = {Id = entity.Id, IngredientName = entity.IngredientName}
            };
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                await _context.EventOutbox.AddAsync(new AggregateEventEntity(@event.ToAggregateEvent()),
                    cancellationToken);
                await _context.Ingredients.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _aggregateEventService.Publish(@event, cancellationToken);

                var response = _mapper.Map<CreateIngredientResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("IX_Ingredients_IngredientName", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(Ingredient), entity.IngredientName);
                }

                throw;
            }
        }
    }
}
