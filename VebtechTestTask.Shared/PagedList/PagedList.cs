namespace VebtechTestTask.Shared.PagedList;

public class PagedList<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int IndexFrom { get; set; }
    public int TotalCount { get; set; }
    public IList<T> Items { get; set; }
    public int TotalPages { get; set; }
}