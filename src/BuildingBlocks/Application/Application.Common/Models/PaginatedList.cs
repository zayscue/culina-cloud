using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CulinaCloud.BuildingBlocks.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int Page { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Page = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int? pageSize)
        {
            var count = await source.CountAsync();
            var take = pageSize.HasValue ? pageSize.Value : count;
            var items = await source.Skip((pageIndex - 1) * take).Take(take).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, take);
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int? pageSize)
        {
            var enumerable = source as T[] ?? source.ToArray();
            var count = enumerable.Count();
            var take = pageSize.HasValue ? pageSize.Value : count;
            var items = enumerable.Skip((pageIndex - 1) * take).Take(take).ToList();

            return new PaginatedList<T>(items, count, pageIndex, take);
        }
    }
}