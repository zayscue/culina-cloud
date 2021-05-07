﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Culina.CookBook.Application.Common.Interfaces;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Application.Common.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Culina.CookBook.Application.Ingredients.Queries.GetIngredients
{
    public class GetIngredientsQuery : IRequest<PaginatedList<GetIngredientsResponse>>
    {
        public string Name { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 1000;
    }

    public class
        GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, PaginatedList<GetIngredientsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetIngredientsQueryHandler> _logger;

        public GetIngredientsQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetIngredientsQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginatedList<GetIngredientsResponse>> Handle(GetIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Ingredients
                .AsNoTracking()
                .OrderBy(x => x.IngredientName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.IngredientName.ToLower(), $"%{request.Name.Trim().ToLower()}%"));
            }


            try
            {
                var response = await query
                    .ProjectTo<GetIngredientsResponse>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Page, request.Limit);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError($"The name of the error is: {nameof(e)}", e);
                throw;
            }

        }
    }
}