using CulinaCloud.Users.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CulinaCloud.BuildingBlocks.Common.Interfaces;

namespace CulinaCloud.Users.Application.Statistics.Queries.GetStatistics
{
    public class GetStatisticsQuery : IRequest<GetStatisticsResponse> {}

    public class GetApplicationUsersStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, GetStatisticsResponse>
    {
        private readonly IApplicationUserManagementService _users;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public GetApplicationUsersStatisticsQueryHandler(IApplicationUserManagementService users, IDateTime dateTime, IMapper mapper)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GetStatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var from = _dateTime.Now.Date.AddDays(-365);
            var to = _dateTime.Now.Date;

            var statistics = await _users.GetApplicationUsersStatistics(from, to, cancellationToken);

            return _mapper.Map<GetStatisticsResponse>(statistics);
        }
    }
}
