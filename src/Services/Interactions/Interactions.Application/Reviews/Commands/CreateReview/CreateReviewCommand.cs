using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
using CulinaCloud.Interactions.Application.Interfaces;
using CulinaCloud.Interactions.Domain.Entities;
using MediatR;

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
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<CreateReviewResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var userId = string.IsNullOrWhiteSpace(request.UserId)
                ? currentUserId 
                : request.UserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("A non empty/null string must be provided as the user id");
            }
            var entity = new Review
            {
                Id = request.Id ?? Guid.NewGuid(),
                UserId = userId,
                RecipeId = request.RecipeId,
                Rating = request.Rating,
                Comments = request.Comments
            };

            await _context.Reviews.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateReviewResponse>(entity);

            return response;
        }
    }
}