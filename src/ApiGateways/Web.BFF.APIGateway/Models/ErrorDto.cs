namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record ErrorDto
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public ExceptionDto? Exception { get; set; }
}

public record ExceptionDto
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Source { get; set; }
}