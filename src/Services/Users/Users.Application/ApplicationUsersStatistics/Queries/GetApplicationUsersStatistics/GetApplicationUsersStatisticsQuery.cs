using CulinaCloud.Users.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Application.ApplicationUsersStatistics.Queries.GetApplicationUsersStatistics
{
    public class GetApplicationUsersStatisticsQuery : IRequest<GetApplicationUsersStatisticsResponse> {}

    public class GetApplicationUsersStatisticsQueryHandler : IRequestHandler<GetApplicationUsersStatisticsQuery, GetApplicationUsersStatisticsResponse>
    {
        private readonly IApplicationUserManagementService _users;

        public GetApplicationUsersStatisticsQueryHandler(IApplicationUserManagementService users)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public async Task<GetApplicationUsersStatisticsResponse> Handle(GetApplicationUsersStatisticsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
