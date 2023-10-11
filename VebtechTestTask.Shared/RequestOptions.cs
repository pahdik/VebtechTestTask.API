using System.Globalization;

namespace VebtechTestTask.Shared;

public class RequestOptions
{
    public int? PageSize { get; set; }
    public int? Page { get; set; }
    public string? Sort { get; set; }
    public string? SortField { get; set; }
    public string? NameFilter { get; set; }
    public string? EmailFilter { get; set; }
    public string? RoleFilter { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}