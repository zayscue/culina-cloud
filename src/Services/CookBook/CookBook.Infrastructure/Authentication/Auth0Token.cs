using System;
using CulinaCloud.CookBook.Application.Common.Interfaces;

namespace CulinaCloud.CookBook.Infrastructure.Authentication
{
    public class Auth0Token
    {
        private readonly IDateTime _dateTime;
        
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring => !string.IsNullOrEmpty(AccessToken) && ExpiresAt > _dateTime.Now.AddSeconds(30);

        public Auth0Token(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }
    }
}