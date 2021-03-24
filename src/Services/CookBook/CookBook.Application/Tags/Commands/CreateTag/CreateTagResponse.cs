using System;
using Culina.CookBook.Application.Common.Mapping;
using Culina.CookBook.Domain.Entities;

namespace Culina.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagResponse : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}