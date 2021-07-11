using System;
using CulinaCloud.CookBook.Application.Common.Mapping;
using CulinaCloud.CookBook.Domain.Entities;

namespace CulinaCloud.CookBook.Application.Tags.Commands.CreateTag
{
    public class CreateTagResponse : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}