namespace PMS.Application.DTOs;

public class PaginationParams 
{
    public int PageNumber { get; set; } = 1; // Default to first page
    public int PageSize { get; set; } = 10;  // Default page size
    public string Search { get; set; } = String.Empty;  // Default page size
}
public class PaginatedList<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    // Calculate total pages
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}