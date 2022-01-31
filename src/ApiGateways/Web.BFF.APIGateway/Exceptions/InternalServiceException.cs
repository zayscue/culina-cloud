namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;

public class InternalServiceException : Exception
{
    public InternalServiceException(string httpErrorResponseContent) : base(httpErrorResponseContent)
    {
        
    }
}