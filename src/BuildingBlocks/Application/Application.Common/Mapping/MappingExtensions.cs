using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CulinaCloud.BuildingBlocks.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.BuildingBlocks.Application.Common.Mapping
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}