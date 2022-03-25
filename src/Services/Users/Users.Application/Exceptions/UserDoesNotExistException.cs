using System;

namespace CulinaCloud.Users.Application.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string userId) : base($"User {userId} does not exist.")
        {
        }
    }
}