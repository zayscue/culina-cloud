﻿using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Culina.CookBook.Application.Common.Interfaces
{
    public interface ITokenServiceManager
    {
        ITokenService GetTokenService(string audience);
    }

    public abstract class BaseTokenServiceManager : ITokenServiceManager
    {
        protected readonly ConcurrentDictionary<string, ITokenService> Services =
            new ConcurrentDictionary<string, ITokenService>();
        public abstract ITokenService GetTokenService(string audience);
    }
}