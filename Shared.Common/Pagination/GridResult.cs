namespace Shared.Common.Pagination;

public class GridResult<T> where T : class
{
    public List<T> Data { get; set; }
    public int Total { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}