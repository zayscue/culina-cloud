using System;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Tags.Queries.GetTag
{
    public class GetTagResponse : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}