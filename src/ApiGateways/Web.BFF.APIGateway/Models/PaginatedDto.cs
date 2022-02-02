namespace CulinaCloud.Web.BFF.APIGateway.Models;

public record PaginatedDto<T>
{
    public List<T>? Items { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set;  }
    public int TotalCount { get; set; }
}
