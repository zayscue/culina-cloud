using System;
using CulinaCloud.CookBook.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.Tags.Queries.GetTags
{
    public class GetTagsResponse : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}