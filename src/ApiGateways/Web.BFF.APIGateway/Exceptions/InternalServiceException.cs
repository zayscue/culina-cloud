﻿namespace CulinaCloud.Web.BFF.APIGateway.Exceptions;

public class InternalServiceException : Exception
{
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public Dictionary<string, string[]>? ValidationErrors { get; set; }

    public InternalServiceException(string serviceName, HttpStatusCode code, string httpErrorResponseContent) : base(GetMessage(serviceName, code, httpErrorResponseContent))
    {
        StatusCode = code;
        if (string.IsNullOrWhiteSpace(httpErrorResponseContent)) return;
        var document = JsonSerializer.Deserialize<InternalServiceError>(httpErrorResponseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new InternalServiceError();
        ErrorCode = document.ErrorCode;
        ErrorMessage = document.Message;
        ValidationErrors = document.ValidationErrors;
    }

    private static string GetMessage(string serviceName, HttpStatusCode code, string httpErrorResponseContent)
    {
        if (string.IsNullOrWhiteSpace(httpErrorResponseContent))
        {
            return $"An internal error has occurred with status code {code} in the {serviceName} service";
        }

        var document = JsonSerializer.Deserialize<InternalServiceError>(httpErrorResponseContent,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }) ?? new InternalServiceError();
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
