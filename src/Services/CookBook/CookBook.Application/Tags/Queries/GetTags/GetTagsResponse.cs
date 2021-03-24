using System;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Tags.Queries.GetTags
{
    public class GetTagsResponse : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}