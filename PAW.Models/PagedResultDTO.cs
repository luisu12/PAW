namespace PAW.Models;

public class PagedResultDTO<T>
{
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
