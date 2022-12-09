namespace CulinaCloud.Web.BFF.APIGateway.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
