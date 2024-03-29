﻿using Auth0.ManagementApi;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
using CulinaCloud.BuildingBlocks.Common.Interfaces;
using CulinaCloud.Users.Application.Interfaces;
using CulinaCloud.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CulinaCloud.Users.Infrastructure.Services
{
    public class Auth0ApplicationUserManagementService : IApplicationUserManagementService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly string _domain;
        private readonly IManagementConnection _managementConnection;

        public Auth0ApplicationUserManagementService(IApplicationDbContext dbContext, ITokenService tokenService, string domain, IManagementConnection managementConnection)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _domain = domain ?? throw new ArgumentNullException(nameof(domain));
            _managementConnection = managementConnection ?? throw new ArgumentNullException(nameof(managementConnection));
        }

        public async Task<ApplicationUser> GetApplicationUser(string userId, CancellationToken cancellation = default)
        {
            var token = await _tokenService.GetToken(cancellation);
            var auth0 = new ManagementApiClient(token.AccessToken, _domain, _managementConnection);

            var entity = await _dbContext.ApplicationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId, cancellation);

            if (entity != null)
            {
                return entity;
            }

            try
            {
                var user = await auth0.Users.GetAsync(userId, cancellationToken: cancellation);
                if (user == null)
                {
                    throw new NotFoundException(nameof(ApplicationUser), userId);
                }
                var newApplicationUser = new ApplicationUser
                {
                    Id = user.UserId,
                    Email = user.Email,
                    DisplayName = user.NickName ?? user.FullName,
                    Picture = user.Picture,
                    CreatedBy = user.UserId
                };
                try
                {
                    await _dbContext.ApplicationUsers.AddAsync(newApplicationUser, cancellation);
                    await _dbContext.SaveChangesAsync(cancellation);
                    return newApplicationUser;
                }
                catch (DbUpdateException)
                {
                    return newApplicationUser;
                }
                catch (Exception)
                {
                    return newApplicationUser;
                }

            }
            catch (Exception)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }
        }

        public async Task<Domain.Entities.Statistics> GetApplicationUsersStatistics(DateTime from, DateTime to, CancellationToken cancellation = default)
        {
            var token = await _tokenService.GetToken(cancellation);
            var auth0 = new ManagementApiClient(token.AccessToken, _domain, _managementConnection);

            var activeUsers = await auth0.Stats.GetActiveUsersAsync(cancellation);
            var dailyUsersInTheLastYear = await auth0.Stats.GetDailyStatsAsync(from, to, cancellation);
            var dailyUsersStatisticsInTheLastYear = dailyUsersInTheLastYear.ToDictionary(x => x.Date, x => new
            {
                SignUps = x.SignUps,
                Logins = x.Logins
            });

            var dailyApplicationUsersStatistics = new List<DailyApplicationUsersStatistics>();
            var current = to;
            while (current > from)
            {
                var currentDateStatistics = new DailyApplicationUsersStatistics
                {
                    Date = current,
                    Logins = 0,
                    SignUps = 0
                };
                if (dailyUsersStatisticsInTheLastYear.ContainsKey(currentDateStatistics.Date))
                {
                    currentDateStatistics.Logins = dailyUsersStatisticsInTheLastYear[currentDateStatistics.Date].Logins;
                    currentDateStatistics.SignUps =
                        dailyUsersStatisticsInTheLastYear[currentDateStatistics.Date].SignUps;
                }
                dailyApplicationUsersStatistics.Add(currentDateStatistics);
                current = current.AddDays(-1);
            }

            return new Statistics
            {
                TotalActiveApplicationUsers = activeUsers,
                DailyApplicationUsersStatistics = dailyApplicationUsersStatistics
            };
        }

        public async Task<ApplicationUser> SaveApplicationUser(ApplicationUser applicationUser, CancellationToken cancellation = default)
        {
            var userId = applicationUser.Id;
            var entity = await _dbContext.ApplicationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId, cancellation);

            if (entity == null)
            {
                if (string.IsNullOrWhiteSpace(applicationUser.CreatedBy))
                {
                    applicationUser.CreatedBy = userId;
                }
                await _dbContext.ApplicationUsers.AddAsync(applicationUser, cancellation);
                await _dbContext.SaveChangesAsync(cancellation);
                entity = applicationUser;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(applicationUser.DisplayName))
                {
                    entity.DisplayName = applicationUser.DisplayName;
                }
                if (!string.IsNullOrWhiteSpace(applicationUser.Picture))
                {
                    entity.Picture = applicationUser.Picture;
                }
                if (string.IsNullOrWhiteSpace(applicationUser.LastModifiedBy))
                {
                    applicationUser.LastModifiedBy = userId;
                }
                else
                {
                    entity.LastModifiedBy = applicationUser.LastModifiedBy;
                }
                _dbContext.ApplicationUsers.Update(entity);
                await _dbContext.SaveChangesAsync(cancellation);
            }

            var updateRequest = new Auth0.ManagementApi.Models.UserUpdateRequest();
            var hasChanged = false;
            if (!string.IsNullOrWhiteSpace(applicationUser.DisplayName))
            {
                updateRequest.NickName = entity.DisplayName;
                hasChanged = true;
            }
            if (!string.IsNullOrWhiteSpace(applicationUser.Picture))
            {
                updateRequest.Picture = entity.Picture;
                hasChanged = true;
            }

            if (hasChanged)
            {
                var token = await _tokenService.GetToken(cancellation);
                var auth0 = new ManagementApiClient(token.AccessToken, _domain, _managementConnection);
                await auth0.Users.UpdateAsync(userId, updateRequest, cancellationToken: cancellation);
            }
            return entity;
        }
    }
}
