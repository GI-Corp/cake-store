namespace Shared.Common.Pagination;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int pageNumber, int pageSize)
    {
        IEnumerable<T> enumerable = items.ToList();
        var count = enumerable.Count();

        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(enumerable);
    }

    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}