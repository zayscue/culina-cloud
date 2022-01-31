namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;

public class InternalServiceAuthorizationException : Exception
{
    public InternalServiceAuthorizationException(string serviceUrl) : base(
        $"An Unauthorized status code was returned while trying to call the {serviceUrl} service.")
    {
    }
}