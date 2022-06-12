﻿using CulinaCloud.BuildingBlocks.Application.Common.Mapping;
using CulinaCloud.Users.Domain.Entities;

namespace CulinaCloud.Users.Application.ApplicationUsers.Queries.GetApplicationUsers
{
    public class GetApplicationUsersResponse : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
    }
}
