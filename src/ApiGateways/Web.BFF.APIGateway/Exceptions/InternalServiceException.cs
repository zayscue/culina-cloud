namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;

public class InternalServiceException : Exception
{
    public string ErrorCode { get; set; }

    public Dictionary<string, string[]> ValidationErrors { get; set; }

    public InternalServiceException(string serviceName, HttpStatusCode code, string httpErrorResponseContent) : base(GetMessage(serviceName, code, httpErrorResponseContent))
    {
        var document = JsonSerializer.Deserialize<InternalServiceError>(httpErrorResponseContent)
            ?? new InternalServiceError();
        ErrorCode = document.ErrorCode;
        ValidationErrors = document.ValidationErrors;
    }

    private static string GetMessage(string serviceName, HttpStatusCode code, string httpErrorResponseContent)
    {
        var document = JsonSerializer.Deserialize<InternalServiceError>(httpErrorResponseContent)
                       ?? new InternalServiceError();
        return string.IsNullOrWhiteSpace(document.Message)
            ? $"An internal error has occurred with status code {code} in the {serviceName} service" 
            : $"An internal error has occurred with status code {code} in the {serviceName} service:{Environment.NewLine}{document.ErrorCode}{Environment.NewLine}{document.Message}";
    }
}

public class InternalServiceError
{
    public string ErrorCode { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();
}
