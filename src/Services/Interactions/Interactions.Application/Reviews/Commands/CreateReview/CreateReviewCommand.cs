using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Common;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Interactions.Application.Exceptions;
using CulinaCloud.Interactions.Application.Interfaces;
using CulinaCloud.Interactions.Domain.Entities;
using CulinaCloud.Interactions.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.Interactions.Application.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<CreateReviewResponse>
    {
        public Guid? Id { get; set; }
        public Guid RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, CreateReviewResponse>
    {
        private readonly IRecipesService _recipesService;
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IAggregateEventService _aggregateEventService;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(
            IRecipesService recipesService,
            IApplicationDbContext context,
            IDateTime dateTime,
            IAggregateEventService aggregateEventService,
            IMapper mapper)
        {
            _recipesService = recipesService;
            _context = context;
            _dateTime = dateTime;
            _aggregateEventService = aggregateEventService;
            _mapper = mapper;
        }

        public async Task<CreateReviewResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTime.Now;
            var userId = request.UserId;
            var entity = new Review
            {
                Id = request.Id ?? Guid.NewGuid(),
                UserId = userId,
                RecipeId = request.RecipeId,
                Rating = request.Rating,
                Comments = request.Comments,
                Created = now,
                CreatedBy = userId
            };
            var @event = new ReviewCreatedEvent
            {
                AggregateId = entity.Id,
                Details = $"A new review was created by {entity.UserId} for recipe {entity.RecipeId} using the POST \"/interactions/reviews\" API.",
                Occurred = entity.Created,
                RaisedBy = entity.CreatedBy,
                Data =
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    RecipeId = entity.RecipeId,
                    Rating = entity.Rating,
                    Comments = entity.Comments
                }
            };
            var recipeServiceIsHealthy = await _recipesService.CheckHealth(cancellationToken);
            if (!recipeServiceIsHealthy)
            {
                throw new RecipeServiceIsNotHealthyException();
            }
            var recipeExists = await _recipesService.RecipeExistsAsync(entity.RecipeId, cancellationToken);
            if (!recipeExists)
            {
                throw new RecipeDoesNotExistException(entity.RecipeId);
            }
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                await _context.EventOutbox.AddAsync(new AggregateEventEntity(@event.ToAggregateEvent()),
                    cancellationToken);
                await _context.Reviews.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _aggregateEventService.Publish(@event, cancellationToken);

                var response = _mapper.Map<CreateReviewResponse>(entity);

                return response;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.Message.Contains("PK_Reviews", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(Review), entity.Id.ToString());
                }

                if (e.InnerException?.Message.Contains("IX_Reviews_UserId_RecipeId", StringComparison.Ordinal) ??
                    false)
                {
                    throw new EntityConflictException(nameof(Review), 
                        JsonSerializer.Serialize(new
                        {
                            entity.UserId,
                            entity.RecipeId
                        }));
                }

                throw;
            }
        }
    }
}