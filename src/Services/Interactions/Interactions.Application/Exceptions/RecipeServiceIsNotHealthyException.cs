using System;

namespace CulinaCloud.Interactions.Application.Exceptions
{
    public class RecipeServiceIsNotHealthyException : Exception
    {
        public RecipeServiceIsNotHealthyException() : base("The recipe service is not healthy")
        {
        }
    }
}
