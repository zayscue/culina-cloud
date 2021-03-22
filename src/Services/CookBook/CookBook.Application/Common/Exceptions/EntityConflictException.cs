using System;

namespace Culina.CookBook.Application.Common.Exceptions
{
    public class EntityConflictException : Exception
    {
        public EntityConflictException(string type, string name)
            : base($"{type} \"{name}\" already exists.")
        {
        }
    }
}
