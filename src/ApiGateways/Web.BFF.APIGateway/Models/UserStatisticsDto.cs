namespace CulinaCloud.Web.BFF.APIGateway.Models
{
    public record UserStatisticsDto
    {
        public long TotalActiveApplicationUsers { get; set; }
        public ICollection<DailyApplicationUsersStatisticsDto> DailyApplicationUsersStatistics { get; set; } = new
            List<DailyApplicationUsersStatisticsDto>();
    }

    public record DailyApplicationUsersStatisticsDto
    {
        public DateTime Date { get; set; }
        public int Logins { get; set; }
        public int SignUps { get; set; }
    }
}
