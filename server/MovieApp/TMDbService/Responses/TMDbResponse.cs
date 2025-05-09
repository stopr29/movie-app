namespace TMDbService.Responses;

public class TMDbResponse<T>
{
    public int Page { get; set; }
    public List<T> Results { get; set; } = new();
    public int TotalResults { get; set; }
    public int TotalPages { get; set; }
}